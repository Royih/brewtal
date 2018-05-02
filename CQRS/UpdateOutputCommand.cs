
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brewtal.BLL;
using Brewtal.Dtos;
using MediatR;

namespace Brewtal.CQRS
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
        protected override void HandleCore(UpdateOutputCommand command)
        {
            _brewIO.Set(command.Output, command.Value);
        }
    }

}
