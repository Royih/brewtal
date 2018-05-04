using System;
using System.Linq;
using System.Threading.Tasks;
using Brewtal.CQRS;
using Brewtal.Database;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Brewtal.BLL.ScheduledWarmup
{
    public class ScheduledWarmupJob : IJob
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScheduledWarmupJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var db = scope.ServiceProvider.GetRequiredService<BrewtalContext>();
                var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<BrewtalHub>>();

                var name = context.JobDetail.Key.Name;
                var brewStepId = int.Parse(name.Replace("job_", ""));

                var brewStep = db.BrewSteps.Include(x => x.Brew).SingleOrDefault(x => x.Id == brewStepId && x.Name == "Initial");
                if (brewStep != null)
                {
                    await mediator.Send(new GoToNextStepCommand
                    {
                        BrewId = brewStep.BrewId
                    });
                    await mediator.Send(new StartLoggingCommand
                    {
                        Name = brewStep.Brew.Name
                    });
                    await hubContext.Clients.All.InvokeAsync("BrewUpdated", brewStep.BrewId);
                    await Console.Out.WriteLineAsync($"{DateTime.Now}: Warmup of brew with name \"{brewStep.Brew.Name}\" and Id={brewStep.BrewId} was initiated.");
                }


            }


        }
    }
}