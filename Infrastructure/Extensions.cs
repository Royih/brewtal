using System;
using Brewtal2.Pid;
using Microsoft.Extensions.DependencyInjection;

namespace Brewtal2.Infrastructure
{
    public static class Extensions
    {

        public static void StartBackgroundWorker(this IServiceProvider serviceProvider)
        {
            var worker = serviceProvider.GetRequiredService<BackgroundWorker>();
            var pidRepo = serviceProvider.GetRequiredService<IPidRepository>();
            worker.Start(pidRepo);
        }
    }
}