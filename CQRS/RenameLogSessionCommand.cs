
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Brewtal.Database
{
    public class RenameLogSessionCommand : IRequest
    {
        public LogSession Session { get; set; }
        public string NewName { get; set; }
    }

    public class RenameLogSessionCommandHandler : IRequestHandler<RenameLogSessionCommand>
    {
        private readonly BrewtalContext _db;

        public RenameLogSessionCommandHandler(BrewtalContext db)
        {
            _db = db;
        }

        public async Task Handle(RenameLogSessionCommand command, CancellationToken cancellationToken)
        {
            var session = _db.Sessions.Single(x => x.Id == command.Session.Id);
            session.Name = command.NewName;
            await _db.SaveChangesAsync();
        }
    }

}
