using Brewtal.BLL;
using Brewtal.BLL.ScheduledWarmup;
using Brewtal.Database;

namespace Brewtal.CQRS
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly BrewtalContext _db;
        private readonly BackgroundWorker _worker;
        private readonly BrewIO _brewIO;
        private readonly ScheduledWarmup _warmupScheduler;

        public AggregateRootFactory(BrewtalContext db, BackgroundWorker worker, BrewIO brewIO, ScheduledWarmup warmupScheduler)
        {
            _db = db;
            _worker = worker;
            _brewIO = brewIO;
            _warmupScheduler = warmupScheduler;
        }

        public BrewAR GetBrewById(int brewId)
        {
            return new BrewAR(_db, _worker, _brewIO, _warmupScheduler, brewId);
        }
    }
}