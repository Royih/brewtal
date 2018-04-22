
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.Database;
using MediatR;

namespace Brewtal.CQRS
{
    public class StopLoggingCommand : IRequest<bool>
    {

    }

    public class StopLoggingCommandHandler : IRequestHandler<StopLoggingCommand, bool>
    {
        private readonly BrewtalContext _db;

        public StopLoggingCommandHandler(BrewtalContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(StopLoggingCommand command, CancellationToken cancellationToken)
        {
            var startedSession = _db.Sessions.SingleOrDefault(x => !x.Completed.HasValue);
            if (startedSession == null)
            {
                throw new Exception("No logging session is started!");
            }
            startedSession.Completed = DateTime.Now;
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
