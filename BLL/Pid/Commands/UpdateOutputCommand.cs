

using Brewtal2.Models.Brews;
using MediatR;

namespace Brewtal2.BLL.Pid.Commands
{
    public class UpdateOutputCommand : IRequest
    {
        public Outputs Output { get; set; }
        public bool Value { get; set; }
    }

    public class UpdateOutputCommandHandler : RequestHandler<UpdateOutputCommand>
    {
        private readonly BrewIO _brewIO;

        public UpdateOutputCommandHandler(BrewIO brewIO)
        {
            _brewIO = brewIO;
        }

        protected override void Handle(UpdateOutputCommand command)
        {
            _brewIO.Set(command.Output, command.Value);
        }

        
    }

}
