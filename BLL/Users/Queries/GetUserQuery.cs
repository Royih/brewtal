using System;
using System.Security;
using Brewtal2.DataAccess;
using Brewtal2.Models;
using MediatR;

namespace Brewtal2.BLL.Users.Queries
{

    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }

    public class GetUserQueryHandler : RequestHandler<GetUserQuery, UserDto>
    {
        private readonly IRepository _repo;
        private readonly ICurrentUser _currentUser;

        public GetUserQueryHandler(IRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        protected override UserDto Handle(GetUserQuery query)
        {
            if (!_currentUser.HasRole(Roles.Admin) && _currentUser.Id != query.Id)
            {
                throw new SecurityException($"User {_currentUser.UserName} does not have access to user with id {query.Id}");
            }
            var t = _repo.GetUser(query.Id).Map();
            return t;
        }
    }
}