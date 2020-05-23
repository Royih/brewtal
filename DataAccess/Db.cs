using System.Security.Authentication;
using Brewtal2.Brews.Models;
using Brewtal2.Infrastructure;
using Brewtal2.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Brewtal2.DataAccess
{
    public class Db : IDb
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMongoClient _client;
        private readonly IMongoCollection<ApplicationUser> _users;
        private readonly IMongoCollection<RefreshToken> _refreshTokens;
        private readonly IMongoCollection<Brew> _brews;
        private readonly IMongoCollection<BrewStepTemplate> _brewStepTemplates;
        private readonly IMongoCollection<LogSession> _logSessions;
        private readonly IMongoCollection<PidConfig> _pidConfigs;

        public Db(IConfiguration config, UserManager<ApplicationUser> userManager)
        {

            var settings = MongoClientSettings.FromUrl(new MongoUrl(config.GetMongoConnectionString()));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _client = new MongoClient(settings);
            var database = _client.GetDatabase(config.GetMongoDatabaseName());
            _users = database.GetCollection<ApplicationUser>("users");
            _userManager = userManager;
            _refreshTokens = database.GetCollection<RefreshToken>("refreshTokens");
            _brews = database.GetCollection<Brew>("brews");
            _brewStepTemplates = database.GetCollection<BrewStepTemplate>("brewStepTemplates");
            _logSessions = database.GetCollection<LogSession>("logSessions");
            _pidConfigs = database.GetCollection<PidConfig>("pidConfigs");
        }

        public IMongoClient Client => _client;
        public IMongoCollection<ApplicationUser> Users => _users;
        public UserManager<ApplicationUser> UserManager => _userManager;
        public IMongoCollection<RefreshToken> RefreshTokens => _refreshTokens;
        public IMongoCollection<Brew> Brews => _brews;
        public IMongoCollection<BrewStepTemplate> BrewStepTemplates => _brewStepTemplates;
        public IMongoCollection<LogSession> LogSessions => _logSessions;
        public IMongoCollection<PidConfig> PidConfigs => _pidConfigs;
    }
}