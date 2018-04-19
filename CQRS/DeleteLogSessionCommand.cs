
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Brewtal.Database
{
    public class DeleteLogSessionCommand : IRequest<bool>
    {
        public LogSession LogSession { get; set; }
    }

    public class DeleteLogSessionCommandHandler : IRequestHandler<DeleteLogSessionCommand, bool>
    {
        private readonly BrewtalContext _db;

        public DeleteLogSessionCommandHandler(BrewtalContext db)
        {
            _db = db;
        }

        public async Task<bool> Handle(DeleteLogSessionCommand command, CancellationToken cancellationToken)
        {
            var session = _db.Sessions.SingleOrDefault(x => x.Id == command.LogSession.Id);
            if (session != null)
            {
                _db.Remove(session);
                await _db.SaveChangesAsync();
            }
            return true;
        }
    }

}
