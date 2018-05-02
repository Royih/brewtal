using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Brewtal.Database;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Brewtal.BLL.ScheduledWarmup
{
    public class ScheduledWarmup
    {
        private readonly IJobFactory _jobFactory;

        public ScheduledWarmup(IJobFactory jobFactory)
        {
            _jobFactory = jobFactory;
        }

        public async void Schedule()
        {
            // Grab the Scheduler instance from the Factory
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" },
                    { "quartz.scheduler.instanceName", "MyScheduler" },
                    { "quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz" },
                    { "quartz.threadPool.threadCount", "3" }
                };

            StdSchedulerFactory factory = new StdSchedulerFactory(props);

            IScheduler scheduler = await factory.GetScheduler();
            scheduler.JobFactory = _jobFactory;
            await scheduler.Clear();
            var futureBrewsSteps = new List<BrewStep>();
            using (var db = new BrewtalContext())
            {
                futureBrewsSteps = db.BrewSteps.Include(x => x.Brew).Where(x => x.Name == "Initial" && x.Brew.BeginMash.AddHours(1) > DateTime.Now).ToList();
            }

            if (futureBrewsSteps.Any())
            {
                // and start it off
                await scheduler.Start();

                foreach (var brewStep in futureBrewsSteps)
                {
                    var startTime = brewStep.Brew.BeginMash.AddHours(-1).ToLocalTime();

                    // define the job and tie it to our HelloJob class
                    IJobDetail job = JobBuilder.Create<ScheduledWarmupJob>()
                        .WithIdentity("job_" + brewStep.Id, "group1")
                        .Build();

                    // Trigger the job to run now, and then repeat every 10 seconds
                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger1", "group1")
                        .StartAt(startTime)
                        /*.WithSimpleSchedule(x => x
                            .WithIntervalInSeconds(10)
                            .RepeatForever())*/
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    await scheduler.ScheduleJob(job, trigger);

                    await Console.Out.WriteLineAsync($"Scheduled warmup of brew {brewStep.Brew.Name} at {startTime}");
                }
            }
            else
            {
                await scheduler.Shutdown();
            }



            // some sleep to show what's happening
            //await Task.Delay(TimeSpan.FromMinutes(3));

            // and last shut down the scheduler when you are ready to close your program
            //await scheduler.Shutdown();
        }
    }
}