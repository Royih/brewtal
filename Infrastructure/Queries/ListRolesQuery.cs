using System.Collections.Generic;
using Brewtal2.Infrastructure.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Queries
{

    public class ListRolesQuery : IRequest<IEnumerable<UserRoleDto>>
    {

    }

    public class ListRolesQueryHandler : RequestHandler<ListRolesQuery, IEnumerable<UserRoleDto>>
    {
        private readonly IAppRepository _repo;

        public ListRolesQueryHandler(IAppRepository repo)
        {
            _repo = repo;
        }

        protected override IEnumerable<UserRoleDto> Handle(ListRolesQuery query)
        {
            return _repo.ListRoles();
        }
    }
}