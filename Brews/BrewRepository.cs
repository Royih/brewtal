using System.Collections.Generic;
using Brewtal2.Brews.Models;
using Brewtal2.DataAccess;
using Brewtal2.Infrastructure.Models;
using MongoDB.Driver;

namespace Brewtal2.Brews
{
    public class BrewRepository : IBrewRepository
    {
        private readonly IDb _db;
        private readonly ICurrentUser _currentUser;

        public BrewRepository(IDb db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public void SeedBrewstepTemplates()
        {

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Initial",
                    CompleteButtonText = "Start Warmup",
                    Instructions = "Get ready for brewing"
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Warmup",
                    CompleteButtonText = "Start adding grain",
                    Instructions = "Wait and relax",
                    Target1TempFrom = "strikeTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Add grain",
                    CompleteButtonText = "Start Mash-timer",
                    Instructions = "Add grain to water in the mash kettle",
                    Target1TempFrom = "mashTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Mash",
                    CompleteButtonText = "Start mash-out",
                    Instructions = "Wait for the timer to reach zero. Stir the mash a few times. Pay attention to the temperature",
                    CompleteTimeAdd = "mashTimeInMinutes",
                    Target1TempFrom = "mashTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Mash out",
                    CompleteButtonText = "Start sparge",
                    Instructions = "Wait for the temperature to reach the critical 75,6ºC",
                    Target1TempFrom = "mashOutTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Sparge",
                    CompleteButtonText = "Sparge complete",
                    Instructions = "Add water to the top of the mash kettle.  Transfer wort from the bottom of the mash kettle to the boil kettle",
                    ShowTimer = true
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Boil warmup",
                    CompleteButtonText = "Start Boil-timer",
                    Instructions = "Wait for the wort to boil. Sample OG (before boil). Note the volume of wort before boil. Take the Yiest out of the fridge now",
                    ShowTimer = true,
                    DataCaptureDefinitions = new []
                    {
                        new DataCaptureDefinition
                        {

                            Label = "OG before boil",
                                ValueType = "int",
                                Optional = false,
                                Units = "SG"
                        },
                        new DataCaptureDefinition
                        {
                            Label = "Wort before boil",
                                ValueType = "float",
                                Optional = false,
                                Units = "l"
                        }
                    }
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Boil",
                    CompleteButtonText = "Start Cool-down",
                    Instructions = "Let the wort boil until timer reaches zero. Add hops according to the hop bill. Add yiest nutrition. Add Whirl-flock (15 minutes before end)",
                    CompleteTimeAdd = "boilTimeInMinutes",
                    ShowTimer = true
            });
            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Cooldown",
                    CompleteButtonText = "Cooldown complete",
                    Instructions = "Cool the wort to 18-20ºC. Use whirlpool to gather remains of hop and grain. Clean the yiest tank now",
                    ShowTimer = true,
                    DataCaptureDefinitions = new []
                    {
                        new DataCaptureDefinition
                        {
                            Label = "OG after boil",
                                ValueType = "int",
                                Optional = false,
                                Units = "SG"
                        }
                    }
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Prepare fermentation",
                    CompleteButtonText = "Begin Fermentation",
                    Instructions = "Transfer to yiest tank(bucket). Note the volume of wort. Add o2. Pitch yiest. Clean up. Be happy:)",
                    ShowTimer = true,
                    DataCaptureDefinitions = new []
                    {
                        new DataCaptureDefinition
                        {
                            Label = "Wort after boil",
                                ValueType = "float",
                                Optional = false,
                                Units = "l"
                        },
                        new DataCaptureDefinition
                        {
                            Label = "Wort to yiest tank",
                                ValueType = "float",
                                Optional = false,
                                Units = "l"
                        }
                    }
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Fermentation",
                    CompleteButtonText = "Archive",
                    Instructions = "Hope. Pray. Dry hop. Whatever.",
                    DataCaptureDefinitions = new []
                    {
                        new DataCaptureDefinition
                        {
                            Label = "FG",
                                ValueType = "int",
                                Optional = false,
                                Units = "SG"
                        }
                    }
            });

            _db.BrewStepTemplates.InsertOne(new BrewStepTemplate
            {
                Name = "Archived",
                    Instructions = "Drink goddamnit!"
            });

        }

        public IEnumerable<Brew> ListBrews()
        {
            return this._db.Brews.Find(x => true).ToEnumerable();
        }

        public Brew GetBrew(string id = null)
        {
            var brew = new Brew();

            return brew;
        }

        public Brew SaveBrew(Brew brewToSave)
        {
            if (string.IsNullOrEmpty(brewToSave.Id))
            {
                _db.Brews.InsertOne(brewToSave);
            }
            else
            {
                _db.Brews.ReplaceOne(x => x.Id == brewToSave.Id, brewToSave);

            }
            return brewToSave;

        }

        public void DeleteBrew(Brew brewToDelete)
        {

        }

    }
}