
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.BLL;
using MediatR;

namespace Brewtal.CQRS
{
    public class UpdatePidConfigCommand : IRequest
    {
        public int PIDId { get; set; }
        public double PIDKp { get; set; }
        public double PIDKi { get; set; }
        public double PIDKd { get; set; }
    }

    public class UpdatePidConfigCommandHandler : RequestHandler<UpdatePidConfigCommand>
    {
        private readonly BackgroundWorker _pidWorker;

        public UpdatePidConfigCommandHandler(BackgroundWorker pidWorker)
        {
            _pidWorker = pidWorker;
        }
        protected override void HandleCore(UpdatePidConfigCommand command)
        {
            _pidWorker.UpdatePidConfig(command.PIDId, command.PIDKp, command.PIDKi, command.PIDKd);
        }
    }

}
