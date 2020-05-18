using Brewtal2.DataAccess;
using Brewtal2.Models;
using MediatR;

namespace Brewtal2.BLL.Users.Queries
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {

    }

    public class GetCurrentUserQueryHandler : RequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IRepository _repo;

        public GetCurrentUserQueryHandler(IRepository repo)
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