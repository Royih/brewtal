
using Brewtal2.Models;
using Brewtal2.Models.Brews;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace Brewtal2.DataAccess
{
    public interface IDb
    {
        IMongoClient Client { get; }
        UserManager<ApplicationUser> UserManager { get; }

        IMongoCollection<RefreshToken> RefreshTokens { get; }
        IMongoCollection<ApplicationUser> Users { get; }

        IMongoCollection<Brew> Brews { get; }
        IMongoCollection<BrewStepTemplate> BrewStepTemplates { get; }
        IMongoCollection<LogSession> LogSessions { get; }
        IMongoCollection<PidConfig> PidConfigs { get; }

    }
}