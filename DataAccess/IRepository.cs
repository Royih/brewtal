using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Brewtal2.Models;

namespace Brewtal2.DataAccess
{
    public interface IRepository
    {
        Task<ClaimsIdentity> GetClaimsIdentity(string userName);

        dynamic GetUserObject(ClaimsIdentity user);

        Task<CommandResultDto> SaveUser(UserDto user, IEnumerable<UserRoleDto> roles = null);
        IEnumerable<UserRoleDto> ListUsersRoles(Guid userId);
        IEnumerable<UserRoleDto> ListRoles();

        Task<CommandResultDto> SeedEmptyDatabase();

        IEnumerable<ApplicationUser> ListUsers();

        ApplicationUser GetUser(Guid id);

        ApplicationUser GetCurrentUser();

        Task<CommandResultDto> CreateUser(UserDto user, string password = null, bool isAdmin = false);

        void AddRefreshToken(string refreshToken, ApplicationUser user, DateTime validTo, string clientIp);
        bool ValidateRefreshToken(string refreshToken, ApplicationUser user);
        bool RemoveRefreshToken(string refreshToken);

    }
}