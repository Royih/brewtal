
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.BLL;
using MediatR;

namespace Brewtal.CQRS
{
    public class UpdatePidTargetCommand : IRequest
    {
        public int PIDId { get; set; }
        public double NewTargetTemp { get; set; }
    }

    public class UpdatePidTargetCommandHandler : RequestHandler<UpdatePidTargetCommand>
    {
        private readonly BackgroundWorker _pidWorker;

        public UpdatePidTargetCommandHandler(BackgroundWorker pidWorker)
        {
            _pidWorker = pidWorker;
        }
        protected override void HandleCore(UpdatePidTargetCommand command)
        {
            _pidWorker.UpdateTargetTemp(command.PIDId, command.NewTargetTemp);
        }
    }

}
