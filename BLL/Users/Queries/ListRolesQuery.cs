using System.Collections.Generic;
using Brewtal2.DataAccess;
using Brewtal2.Models;
using MediatR;

namespace Brewtal2.BLL.Users.Queries
{

    public class ListRolesQuery : IRequest<IEnumerable<UserRoleDto>>
    {

    }

    public class ListRolesQueryHandler : RequestHandler<ListRolesQuery, IEnumerable<UserRoleDto>>
    {
        private readonly IRepository _repo;

        public ListRolesQueryHandler(IRepository repo)
        {
            _repo = repo;
        }

        protected override IEnumerable<UserRoleDto> Handle(ListRolesQuery query)
        {
            return _repo.ListRoles();
        }
    }
}