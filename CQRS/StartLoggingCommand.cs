
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.Database;
using MediatR;

namespace Brewtal.CQRS
{
    public class StartLoggingCommand : IRequest<bool>
    {
        public string Name { get; set; }
    }

    public class StartLoggingCommandHandler : IRequestHandler<StartLoggingCommand, bool>
    {
        private readonly BrewtalContext _db;

        public StartLoggingCommandHandler(BrewtalContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(StartLoggingCommand command, CancellationToken cancellationToken)
        {
            var currentSession = _db.Sessions.FirstOrDefault(x => !x.Completed.HasValue);
            if (currentSession != null)
            {
                currentSession.Completed = DateTime.Now;
            }
            var session = new LogSession
            {
                Created = DateTime.Now,
                Name = command.Name
            };
            _db.Add(session);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
