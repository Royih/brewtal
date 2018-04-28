using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.Database
{
    public static class BrewtalContextExtension
    {
        public static void Seed(this BrewtalContext db)
        {
            if (db.BrewStepTemplates.Count() == 0)
            {

                var initialStep = new BrewStepTemplate
                {
                    Name = "Initial",
                    CompleteButtonText = "Start Warmup",
                    Instructions = "Get ready for brewing"
                };
                db.Add(initialStep);
                db.Add(new BrewStepTemplate
                {
                    Name = "Warmup",
                    CompleteButtonText = "Start adding grain",
                    Instructions = "Wait and relax",
                    Target1TempFrom = "strikeTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
                });
                db.Add(new BrewStepTemplate
                {
                    Name = "Add grain",
                    CompleteButtonText = "Start Mash-timer",
                    Instructions = "Add grain to water in the mash kettle",
                    Target1TempFrom = "mashTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
                });
                db.Add(new BrewStepTemplate
                {
                    Name = "Mash",
                    CompleteButtonText = "Start mash-out",
                    Instructions = "Wait for the timer to reach zero. Stir the mash a few times. Pay attention to the temperature",
                    CompleteTimeAdd = "mashTimeInMinutes",
                    Target1TempFrom = "mashTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
                });
                db.Add(new BrewStepTemplate
                {
                    Name = "Mash out",
                    CompleteButtonText = "Start sparge",
                    Instructions = "Wait for the temperature to reach the critical 75,6ºC",
                    Target1TempFrom = "mashOutTemp",
                    Target2TempFrom = "spargeTemp",
                    ShowTimer = true
                });
                db.Add(new BrewStepTemplate
                {
                    Name = "Sparge",
                    CompleteButtonText = "Sparge complete",
                    Instructions = "Add water to the top of the mash kettle.  Transfer wort from the bottom of the mash kettle to the boil kettle",
                    ShowTimer = true
                });
                var boilWarmupStep = new BrewStepTemplate
                {
                    Name = "Boil warmup",
                    CompleteButtonText = "Start Boil-timer",
                    Instructions = "Wait for the wort to boil. Sample OG (before boil). Note the volume of wort before boil. Take the Yiest out of the fridge now",
                    ShowTimer = true
                };
                db.Add(boilWarmupStep);
                db.Add(new BrewStepTemplate
                {
                    Name = "Boil",
                    CompleteButtonText = "Start Cool-down",
                    Instructions = "Let the wort boil until timer reaches zero. Add hops according to the hop bill. Add yiest nutrition. Add Whirl-flock (15 minutes before end)",
                    CompleteTimeAdd = "boilTimeInMinutes",
                    ShowTimer = true
                });
                var cooldownStep = new BrewStepTemplate
                {
                    Name = "Cooldown",
                    CompleteButtonText = "Cooldown complete",
                    Instructions = "Cool the wort to 18-20ºC. Use whirlpool to gather remains of hop and grain. Clean the yiest tank now",
                    ShowTimer = true
                };
                db.Add(cooldownStep);
                var prepareFermentationStep = new BrewStepTemplate
                {
                    Name = "Prepare fermentation",
                    CompleteButtonText = "Begin Fermentation",
                    Instructions = "Transfer to yiest tank(bucket). Note the volume of wort. Add o2. Pitch yiest. Clean up. Be happy:)",
                    ShowTimer = true
                };
                db.Add(prepareFermentationStep);
                var fermentationStep = new BrewStepTemplate
                {
                    Name = "Fermentation",
                    CompleteButtonText = "Archive",
                    Instructions = "Hope. Pray. Dry hop. Whatever.",
                };
                db.Add(fermentationStep);
                db.Add(new BrewStepTemplate
                {
                    Name = "Archived",
                    Instructions = "Drink goddamnit!"
                });

                db.Add(new DataCaptureDefinition
                {
                    BrewStepTemplate = boilWarmupStep,
                    Label = "OG before boil",
                    ValueType = "int",
                    Optional = false,
                    Units = "SG"
                });
                db.Add(new DataCaptureDefinition
                {
                    BrewStepTemplate = boilWarmupStep,
                    Label = "Wort before boil",
                    ValueType = "float",
                    Optional = false,
                    Units = "l"
                });
                db.Add(new DataCaptureDefinition
                {
                    BrewStepTemplate = cooldownStep,
                    Label = "OG after boil",
                    ValueType = "int",
                    Optional = false,
                    Units = "SG"
                });
                db.Add(new DataCaptureDefinition
                {
                    BrewStepTemplate = prepareFermentationStep,
                    Label = "Wort after boil",
                    ValueType = "float",
                    Optional = false,
                    Units = "l"
                });
                db.Add(new DataCaptureDefinition
                {
                    BrewStepTemplate = prepareFermentationStep,
                    Label = "Wort to yiest tank",
                    ValueType = "float",
                    Optional = false,
                    Units = "l"
                });
                db.Add(new DataCaptureDefinition
                {
                    BrewStepTemplate = fermentationStep,
                    Label = "FG",
                    ValueType = "int",
                    Optional = false,
                    Units = "SG"
                });

                db.SaveChanges();
            }

        }
    }

}
