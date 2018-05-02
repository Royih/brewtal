using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Brewtal.BLL.ScheduledWarmup
{

    public class JobFactory : IJobFactory
    {
        protected readonly IServiceScopeFactory _serviceScopeFactory;

        public JobFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return new ScheduledWarmupJob(_serviceScopeFactory);
        }

        public void ReturnJob(IJob job)
        {
            // i couldn't find a way to release services with your preferred DI, 
            // its up to you to google such things
        }

    }
}
