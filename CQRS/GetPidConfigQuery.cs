
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.BLL;
using Brewtal.Database;
using Brewtal.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Brewtal.CQRS
{
    public class GetPidConfigQuery : IRequest<PidConfig>
    {
        public int PidId { get; set; }
    }

    public class GetPidConfigQueryHandler : RequestHandler<GetPidConfigQuery, PidConfig>
    {
        private readonly PidWorker _pidWorker;

        public GetPidConfigQueryHandler(PidWorker pidWorker)
        {
            _pidWorker = pidWorker;
        }


        protected override PidConfig HandleCore(GetPidConfigQuery query)
        {
            return _pidWorker.GetConfig(query.PidId);
        }
    }

}
