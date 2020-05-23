using System;
using System.Collections.Generic;
using System.Security;
using Brewtal2.Infrastructure.Models;
using MediatR;

namespace Brewtal2.Infrastructure.Queries
{

    public class ListUserRolesQuery : IRequest<IEnumerable<UserRoleDto>>
    {
        public Guid Id { get; set; }
    }

    public class ListUserRolesQueryHandler : RequestHandler<ListUserRolesQuery, IEnumerable<UserRoleDto>>
    {
        private readonly IAppRepository _repo;
        private readonly ICurrentUser _currentUser;

        public ListUserRolesQueryHandler(IAppRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        protected override IEnumerable<UserRoleDto> Handle(ListUserRolesQuery query)
        {
            if (!_currentUser.HasRole(Roles.Admin) && _currentUser.Id != query.Id)
            {
                throw new SecurityException($"User {_currentUser.UserName} does not have access to user with id {query.Id}");
            }
            return _repo.ListUsersRoles(query.Id);
        }
    }
}