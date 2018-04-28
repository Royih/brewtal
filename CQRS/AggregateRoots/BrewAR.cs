using System;
using System.Linq;
using Brewtal.Database;
using Brewtal.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{
    public class BrewAR
    {
        private readonly BrewtalContext _db;
        private readonly int _brewId;

        public BrewAR(BrewtalContext db, int brewId)
        {
            _db = db;
            _brewId = brewId;
        }

        public Brew SaveBrew(BrewDto value)
        {
            Brew brew = null;
            if (value.Id > 0)
            {
                brew = _db.Brews.Single(x => x.Id == value.Id);
            }
            else
            {
                brew = new Brew();
                _db.Add(brew);

                var firstStep = GetFirstStep(brew);

                var brewStep = AddStep(brew, firstStep);
            }
            brew.Initiated = DateTime.UtcNow;
            brew.BeginMash = value.BeginMash.ToUniversalTime();
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
            _db.SaveChanges();
            return brew;
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
            //TODO: 
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