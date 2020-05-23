using MediatR;

namespace Brewtal2.Pid.Commands
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
        protected override void Handle(UpdatePidTargetCommand command)
        {
            _pidWorker.UpdateTargetTemp(command.PIDId, command.NewTargetTemp);
        }
    }

}