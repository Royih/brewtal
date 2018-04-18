
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.Database
{
    public class ListSessionsQuery : IRequest<IEnumerable<LogSession>>
    {

    }

    public class ListSessionsQueryHandler : IRequestHandler<ListSessionsQuery, IEnumerable<LogSession>>
    {
        private readonly BrewtalContext _db;

        public ListSessionsQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<LogSession>> Handle(ListSessionsQuery query, CancellationToken cancellationToken)
        {
            return await _db.Sessions.ToArrayAsync();
        }
    }

}
