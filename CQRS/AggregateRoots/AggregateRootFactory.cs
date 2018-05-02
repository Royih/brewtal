using Brewtal.BLL;
using Brewtal.Database;

namespace Brewtal.CQRS
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly BrewtalContext _db;
        private readonly BackgroundWorker _worker;
        private readonly BrewIO _brewIO;

        public AggregateRootFactory(BrewtalContext db, BackgroundWorker worker, BrewIO brewIO)
        {
            _db = db;
            _worker = worker;
            _brewIO = brewIO;
        }

        public BrewAR GetBrewById(int brewId)
        {
            return new BrewAR(_db, _worker, _brewIO, brewId);
        }
    }
}