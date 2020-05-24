using Brewtal2.DataAccess;
using Brewtal2.Infrastructure.Models;
using Brewtal2.Pid.Models;
using MongoDB.Driver;

namespace Brewtal2.Pid
{
    public class PidRepository : IPidRepository
    {

        private readonly IDb _db;
        private readonly ICurrentUser _currentUser;

        public PidRepository(IDb db, ICurrentUser currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }

        public void AddPidConfig(PidConfig pidConfig)
        {
            _db.PidConfigs.InsertOne(pidConfig);
        }

        public PidConfig GetPidConfig(int pidId)
        {
            return _db.PidConfigs.Find(x => x.PidId == pidId).SingleOrDefault();
        }

        public void UpdateExistingPidConfig(PidConfig pidConfig)
        {
            _db.PidConfigs.ReplaceOne(x => x.Id == pidConfig.Id, pidConfig);
        }
    }
}