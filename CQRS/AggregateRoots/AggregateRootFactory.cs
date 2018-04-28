using Brewtal.Database;

namespace Brewtal.CQRS
{
    public class AggregateRootFactory : IAggregateRootFactory
    {
        private readonly BrewtalContext _db;
        public AggregateRootFactory(BrewtalContext db)
        {
            _db = db;
        }

        public BrewAR GetBrewById(int brewId)
        {
            return new BrewAR(_db, brewId);
        }
    }
}