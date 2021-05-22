using MediatR;

namespace Brewtal2.Pid.Commands
{
    public class UpdatePidModeCommand : IRequest
    {
        public bool FridgeMode { get; set; }
    }

    public class UpdatePidModeCommandHandler : RequestHandler<UpdatePidModeCommand>
    {
        private readonly BackgroundWorker _pidWorker;

        public UpdatePidModeCommandHandler(BackgroundWorker pidWorker)
        {
            _pidWorker = pidWorker;
        }
        protected override void Handle(UpdatePidModeCommand command)
        {
            if (command.FridgeMode)
            {
                _pidWorker.SetFridgeMode();
            }
            else
            {
                _pidWorker.SetPidMode();
            }
        }
    }

}