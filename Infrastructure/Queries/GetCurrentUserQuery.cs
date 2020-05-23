
using Brewtal2.Infrastructure.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Queries
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {

    }

    public class GetCurrentUserQueryHandler : RequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IAppRepository _repo;

        public GetCurrentUserQueryHandler(IAppRepository repo)
        {
            _repo = repo;
        }

        protected override UserDto Handle(GetCurrentUserQuery query)
        {
            var t = _repo.GetCurrentUser().Map();
            return t;
        }
    }
}