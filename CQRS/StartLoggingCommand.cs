
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Brewtal.Database
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
            if (_db.Sessions.Any(x => !x.Completed.HasValue))
            {
                throw new Exception("Logging session already started!");
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
