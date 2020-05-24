using System;
using System.Linq;
using System.Threading.Tasks;
using Brewtal2.Brews;
using Brewtal2.DataAccess;
using Brewtal2.Infrastructure.Models;
using Brewtal2.Pid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Serilog;

namespace Brewtal2.Infrastructure
{
    public static class SeedDBExtensions
    {
        private static IDb _db;

        private static IConfiguration _configuration;

        private static async Task<CommandResultDto> DoSeedDb(this IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _db = serviceProvider.GetRequiredService<IDb>();
            var repository = serviceProvider.GetRequiredService<IAppRepository>();
            var brewRepository = serviceProvider.GetRequiredService<IBrewRepository>();
            _configuration = configuration;
            if (!_db.UserManager.Users.Any())
            {
                brewRepository.SeedBrewstepTemplates();
                return await repository.SeedEmptyDatabase();
            }

            Log.Information("DB already seeded.");
            return new CommandResultDto { Success = true };
        }

        public static void SeedDb(this IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var res = serviceProvider.DoSeedDb(configuration).GetAwaiter().GetResult();
            if (!res.Success)
            {
                Log.Error("Error seeding DB.");
                foreach (var err in res.ErrorMessages)
                {
                    Log.Error(err);
                }
            }
            if (res.Messages != null)
            {
                foreach (var msg in res.Messages)
                {
                    Log.Information(msg);
                }
            }
        }

        public static void StartBackgroundWorker(this IServiceProvider serviceProvider)
        {
            var worker = serviceProvider.GetRequiredService<BackgroundWorker>();
            var pidRepo = serviceProvider.GetRequiredService<IPidRepository>();
            worker.Start(pidRepo);
        }
    }
}