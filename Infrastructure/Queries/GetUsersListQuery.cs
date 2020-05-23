using System.Collections.Generic;
using System.Linq;
using Brewtal2.Infrastructure.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Queries
{

    public class GetUsersListQuery : IRequest<IEnumerable<UserDto>>
    { }

    public class GetUsersListQueryHandler : RequestHandler<GetUsersListQuery, IEnumerable<UserDto>>
    {
        private readonly IAppRepository _repo;

        public GetUsersListQueryHandler(IAppRepository repo)
        {
            _repo = repo;
        }

        protected override IEnumerable<UserDto> Handle(GetUsersListQuery query)
        {
            var t = _repo.ListUsers().Select(x => x.Map());
            return t;
        }
    }
}