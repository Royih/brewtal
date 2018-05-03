using System;
using System.Linq;
using AutoMapper;
using Brewtal.BLL;
using Brewtal.BLL.ScheduledWarmup;
using Brewtal.Database;
using Brewtal.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{
    public class BrewAR
    {
        private readonly BrewtalContext _db;
        private readonly BackgroundWorker _worker;
        private readonly BrewIO _brewIO;
        private readonly ScheduledWarmup _warmupScheduler;

        private readonly int _brewId;

        public BrewAR(BrewtalContext db, BackgroundWorker worker, BrewIO brewIO, ScheduledWarmup warmupScheduler, int brewId)
        {
            _db = db;
            _brewId = brewId;
            _worker = worker;
            _brewIO = brewIO;
            _warmupScheduler = warmupScheduler;
        }

        public Brew SaveBrew(BrewDto value)
        {
            var beginMashHasChanged = false;
            Brew brew = null;
            if (value.Id > 0)
            {
                brew = _db.Brews.Single(x => x.Id == value.Id);

                if (!string.IsNullOrEmpty(brew.OptimisticConcurrencyKey))
                {
                    if (value.OptimisticConcurrencyKey != brew.OptimisticConcurrencyKey)
                    {
                        throw new ApplicationException("Concurrency error. Brew has been saved after you loaded it.");
                    }
                }

                beginMashHasChanged = brew.BeginMash != value.BeginMash;
            }
            else
            {
                brew = new Brew();
                beginMashHasChanged = true;
                _db.Add(brew);

                var firstStep = GetFirstStep(brew);

                var brewStep = AddStep(brew, firstStep);
            }
            brew.Initiated = DateTime.UtcNow;
            brew.BeginMash = value.BeginMash.AddSeconds((-1) * value.BeginMash.Second);
            brew.BatchNumber = value.BatchNumber;
            brew.Name = value.Name;
            brew.MashTemp = value.MashTemp;
            brew.StrikeTemp = value.StrikeTemp;
            brew.SpargeTemp = value.SpargeTemp;
            brew.MashOutTemp = value.MashOutTemp;
            brew.MashTimeInMinutes = value.MashTimeInMinutes;
            brew.BoilTimeInMinutes = value.BoilTimeInMinutes;
            brew.BatchSize = value.BatchSize;
            brew.MashWaterAmount = value.MashWaterAmount;
            brew.SpargeWaterAmount = value.SpargeWaterAmount;
            brew.Notes = value.Notes;
            brew.ShoppingList = value.ShoppingList;
            brew.OptimisticConcurrencyKey = Guid.NewGuid().ToString();

            _db.SaveChanges();

            if (beginMashHasChanged)
            {
                _warmupScheduler.Schedule();
            }

            return brew;
        }

        public Brew SaveBrewNotes(string notes)
        {
            var brew = Mapper.Map<BrewDto>(_db.Brews.Single(x => x.Id == _brewId));
            brew.Notes = notes;
            return SaveBrew(brew);

        }

        public Brew SaveShoppingList(string shoppingList)
        {
            var brew = Mapper.Map<BrewDto>(_db.Brews.Single(x => x.Id == _brewId));
            brew.ShoppingList = shoppingList;
            return SaveBrew(brew);
        }

        public void GoToNextStep()
        {
            var brewStep = _db.BrewSteps.Include(x => x.Brew).OrderByDescending(x => x.Order).FirstOrDefault(x => x.BrewId == _brewId);

            var nextStep = GetNextStep(brewStep);

            if (nextStep != null)
            {
                AddStep(brewStep.Brew, nextStep);
            }
            _db.SaveChanges();
        }

        public void GoBackOneStep()
        {
            var logs = _db.BrewSteps.Where(x => x.BrewId == _brewId).OrderByDescending(x => x.Order);
            if (logs.Count() > 1) //we dont want to delete the last row
            {
                _db.Remove(logs.First());
            }
            ApplyStepTemperature(logs.ToList()[1]);
            _db.SaveChanges();
        }

        private BrewStep AddStep(Brew log, StepDto step)
        {
            var brewStep = new BrewStep
            {
                Brew = log,
                Order = step.Order,
                Name = step.Name,
                StartTime = DateTime.UtcNow,
                CompleteTime = step.CompleteTime,
                TargetMashTemp = step.TargetMashTemp,
                TargetSpargeTemp = step.TargetSpargeTemp,
                CompleteButtonText = step.CompleteButtonText,
                Instructions = step.Instructions,
                ShowTimer = step.ShowTimer
            };
            _db.Add(brewStep);

            ApplyStepTemperature(brewStep);

            //Add DataCapture values
            var dataCaptureValues = _db.DataCaptureDefinitions.Where(x => x.BrewStepTemplateId == step.Order);
            foreach (var v in dataCaptureValues)
            {
                if (v.ValueType == "float")
                {
                    _db.Add(new DataCaptureFloatValue
                    {
                        BrewStep = brewStep,
                        Label = v.Label,
                        Optional = v.Optional,
                        Units = v.Units
                    });
                }
                else if (v.ValueType == "int")
                {
                    _db.Add(new DataCaptureIntValue
                    {
                        BrewStep = brewStep,
                        Label = v.Label,
                        Optional = v.Optional,
                        Units = v.Units
                    });
                }
                else if (v.ValueType == "string")
                {
                    _db.Add(new DataCaptureStringValue
                    {
                        BrewStep = brewStep,
                        Label = v.Label,
                        Optional = v.Optional,
                        Units = v.Units
                    });
                }
            }

            return brewStep;
        }


        private void ApplyStepTemperature(BrewStep brewStep)
        {
            _worker.UpdateTargetTemp(0, brewStep.TargetMashTemp);
            if (brewStep.TargetMashTemp > 0)
            {
                _brewIO.Set(Outputs.Output1, true);
            }
            _worker.UpdateTargetTemp(1, brewStep.TargetSpargeTemp);
            if (brewStep.TargetSpargeTemp > 0)
            {
                _brewIO.Set(Outputs.Output2, true);
            }
        }

        private StepDto GetFirstStep(Brew brew)
        {
            return GetStepDto(_db.BrewStepTemplates.OrderBy(x => x.Id).First(), brew);
        }
        private StepDto GetNextStep(BrewStep brewStep)
        {
            return GetStepDto(_db.BrewStepTemplates.Where(x => x.Id > brewStep.Order).First(), brewStep.Brew);
        }

        private StepDto GetStepDto(BrewStepTemplate template, Brew brew)
        {
            return new StepDto
            {
                Order = template.Id,
                Name = template.Name,
                StartTime = DateTime.UtcNow,
                CompleteButtonText = template.CompleteButtonText,
                Instructions = template.Instructions,
                CompleteTime = ResolveCompleteTime(brew, template.CompleteTimeAdd),
                TargetMashTemp = ResolveTemp(brew, template.Target1TempFrom),
                TargetSpargeTemp = ResolveTemp(brew, template.Target2TempFrom),
                ShowTimer = template.ShowTimer
            };

        }

        private DateTime? ResolveCompleteTime(Brew brew, string placeHolder)
        {
            if (placeHolder == "mashTimeInMinutes")
            {
                return DateTime.UtcNow.AddMinutes(brew.MashTimeInMinutes);
            }
            if (placeHolder == "boilTimeInMinutes")
            {
                return DateTime.UtcNow.AddMinutes(brew.BoilTimeInMinutes);
            }
            return null;
        }
        private float ResolveTemp(Brew brew, string placeHolder)
        {
            if (placeHolder == "strikeTemp")
            {
                return brew.StrikeTemp;
            }
            if (placeHolder == "spargeTemp")
            {
                return brew.SpargeTemp;
            }
            if (placeHolder == "mashTemp")
            {
                return brew.MashTemp;
            }
            if (placeHolder == "mashOutTemp")
            {
                return brew.MashOutTemp;
            }
            return 0;
        }


    }
}