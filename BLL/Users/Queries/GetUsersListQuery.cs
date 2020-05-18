using System.Collections.Generic;
using System.Linq;
using Brewtal2.DataAccess;
using Brewtal2.Models;
using MediatR;

namespace Brewtal2.BLL.Users.Queries
{

    public class GetUsersListQuery : IRequest<IEnumerable<UserDto>>
    { }

    public class GetUsersListQueryHandler : RequestHandler<GetUsersListQuery, IEnumerable<UserDto>>
    {
        private readonly IRepository _repo;

        public GetUsersListQueryHandler(IRepository repo)
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