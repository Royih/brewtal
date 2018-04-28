
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{
    public class GetLogSessionQuery : IRequest<LogSessionDto>
    {
        public int SessionId { get; set; }
    }

    public class GetLogSessionQueryHandler : RequestHandler<GetLogSessionQuery, LogSessionDto>
    {
        private readonly BrewtalContext _db;

        public GetLogSessionQueryHandler(BrewtalContext db)
        {
            _db = db;
        }

        protected override LogSessionDto HandleCore(GetLogSessionQuery query)
        {
            var session = _db.Sessions.Single(x => x.Id == query.SessionId);
            return new LogSessionDto { Id = session.Id, Name = session.Name, Created = session.Created, Completed = session.Completed, LogPoints = _db.Records.Count(x => x.SessionId == query.SessionId) };
        }
    }

}
