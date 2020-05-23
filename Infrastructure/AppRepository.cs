using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using Brewtal2.DataAccess;
using Brewtal2.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Brewtal2.Infrastructure
{
    public class AppRepository : IAppRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IDb _db;
        private readonly ICurrentUser _currentUser;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<MongoIdentityRole> _roleManager;

        public AppRepository(IConfiguration configuration, IDb db, ICurrentUser currentUser, UserManager<ApplicationUser> userManager, RoleManager<MongoIdentityRole> roleManager)
        {
            _configuration = configuration;
            _db = db;
            _currentUser = currentUser;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        //todo: Seperate repo for Security stuff?
        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName)
        {
            var usr = await _userManager.FindByNameAsync(userName);

            var id = new ClaimsIdentity(new GenericIdentity(userName, "Token"));
            id.AddClaim(new Claim("id", usr.Id.ToString()));
            var roles = await _userManager.GetRolesAsync(usr);
            foreach (var roleName in roles)
            {
                id.AddClaim(new Claim("role", roleName));
            }
            return id;
        }

        dynamic IAppRepository.GetUserObject(ClaimsIdentity user)
        {
            return new
            {
                name = user.Name,
                    claims = user.Claims.Select(x => new { type = x.Type.ToString(), value = x.Value })
            };
        }

        public IEnumerable<ApplicationUser> ListUsers()
        {
            return _db.UserManager.Users.OrderBy(x => x.FullName);
        }

        public ApplicationUser GetUser(Guid id)
        {
            return _db.UserManager.Users.Single(x => x.Id == id);
        }

        public ApplicationUser GetCurrentUser()
        {
            return _db.UserManager.Users.Single(x => x.UserName == _currentUser.UserName);
        }

        public async Task<CommandResultDto> SeedEmptyDatabase()
        {
            var messages = new List<string>();
            var errorMessages = new List<string>();
            await SeedRoles();
            messages.Add("Successfully seeded roles");

            var user = new UserDto
            {
                FullName = "Roy Ingar Hansen",
                UserName = "rihansen@gmail.com",
                Culture = Constants.ListCultures.Single(x => x.Key == "nb-NO")
            };
            var createUserResult = await CreateUser(user, _configuration["DefaultPassword"], true);
            messages.AddRange(createUserResult.Messages);
            errorMessages.AddRange(createUserResult.ErrorMessages);

            return new CommandResultDto() { Messages = messages.ToArray(), ErrorMessages = errorMessages.ToArray(), Success = !errorMessages.Any() };
        }

        private async Task SeedRoles()
        {
            await _roleManager.CreateAsync(new MongoIdentityRole { Id = Guid.NewGuid(), Name = "User" });
            await _roleManager.CreateAsync(new MongoIdentityRole { Id = Guid.NewGuid(), Name = "Admin" });
        }

        public async Task<CommandResultDto> CreateUser(UserDto user, string password = null, bool isAdmin = false)
        {
            var messages = new List<string>();
            var errorMessages = new List<string>();

            var userNameAndEmail = user.UserName;
            var newUser = new ApplicationUser
            {
                UserName = userNameAndEmail,
                Culture = user.Culture.Key,
                Email = userNameAndEmail,
                FullName = user.FullName
            };

            var t = await _userManager.CreateAsync(newUser, password);

            if (t.Succeeded)
            {
                var userCreated = _userManager.Users.Single(x => x.UserName == userNameAndEmail);
                messages.Add($"User with Id {userCreated.Id} seeded successfully");
                await _userManager.AddToRoleAsync(newUser, "User");
                messages.Add($"User role granted successfully");
                if (isAdmin)
                {
                    messages.Add($"Admin role granted successfully");
                    await _userManager.AddToRoleAsync(newUser, "Admin");
                }
                return new CommandResultDto() { Success = true, Messages = messages.ToArray(), ErrorMessages = errorMessages.ToArray(), Data = userCreated };

            }
            return new CommandResultDto { Success = false, ErrorMessages = t.Errors.Select(x => x.Description).ToArray(), Messages = messages.ToArray() };
        }

        public async Task<CommandResultDto> SaveUser(UserDto newUser, IEnumerable<UserRoleDto> roles = null)
        {
            var user = await _userManager.FindByIdAsync(newUser.Id.ToString());
            user.FullName = newUser.FullName;
            user.Culture = newUser.Culture.Key;

            await _userManager.UpdateAsync(user);

            if (user.UserName != newUser.UserName)
            {
                var usr = await _userManager.FindByIdAsync(user.Id.ToString());
                await _userManager.SetUserNameAsync(usr, newUser.UserName);
                await _userManager.SetEmailAsync(usr, newUser.UserName);
            }

            //Sync roles
            if (roles != null)
            {
                var rolesNow = _roleManager.Roles.ToList().Where(x => user.Roles.Any(y => y == x.Id));
                var rolesNew = _roleManager.Roles.ToList().Where(x => roles.Any(y => y.Name == x.Name && y.Selected));

                foreach (var missingRole in rolesNew.Where(x => !rolesNow.Any(y => y.Id == x.Id)))
                {
                    var t = await _userManager.AddToRoleAsync(user, missingRole.Name);
                    if (!t.Succeeded)
                    {
                        throw new Exception(string.Join(",", t.Errors));
                    }
                }
                foreach (var extraRole in rolesNow.Where(x => !rolesNew.Any(y => y.Id == x.Id)))
                {
                    var t = await _userManager.RemoveFromRoleAsync(user, extraRole.Name);
                    if (!t.Succeeded)
                    {
                        throw new Exception(string.Join(",", t.Errors));
                    }
                }
            }
            return new CommandResultDto() { Success = true, Data = this.GetUser(newUser.Id) };
        }
        public IEnumerable<UserRoleDto> ListUsersRoles(Guid userId)
        {
            var user = GetUser(userId);
            return _roleManager.Roles.ToList().Select(x => new UserRoleDto { Name = x.Name, Selected = user.Roles.Any(y => y == x.Id) });
        }

        public IEnumerable<UserRoleDto> ListRoles()
        {
            return _roleManager.Roles.ToList().Select(x => new UserRoleDto { Name = x.Name, Selected = x.Name == "User" });
        }

        public void AddRefreshToken(string refreshTokenEncoded, ApplicationUser user, DateTime validTo, string clientIp)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                ClientIp = clientIp,
                ExpiresUtc = validTo,
                IssuedUtc = DateTime.UtcNow,
                ProtectedTicket = refreshTokenEncoded,
            };
            _db.RefreshTokens.InsertOne(refreshToken);
            _db.RefreshTokens.DeleteMany(x => x.ExpiresUtc < DateTime.UtcNow);
        }
        public bool ValidateRefreshToken(string refreshToken, ApplicationUser user)
        {
            var existing = _db.RefreshTokens.Find(x => x.UserId == user.Id && x.ProtectedTicket == refreshToken && x.ExpiresUtc > DateTime.UtcNow).SingleOrDefault();
            if (existing != null)
            {
                // Update last used on the refresh token
                existing.LastUsed = DateTime.UtcNow;
                _db.RefreshTokens.ReplaceOne(x => x.Id == existing.Id, existing);

                return true;
            }
            return false;
        }

        public bool RemoveRefreshToken(string refreshToken)
        {
            var existing = _db.RefreshTokens.Find(x => x.ProtectedTicket == refreshToken).SingleOrDefault();
            if (existing != null)
            {
                _db.RefreshTokens.DeleteOne(x => x.ProtectedTicket == refreshToken);
                return true;
            }
            return false;
        }

        public static IEnumerable<CultureDto> ListCultures
        {
            get
            {
                yield return new CultureDto { Key = "nb-NO", Value = "Norsk" };
                yield return new CultureDto { Key = "en-US", Value = "English" };
            }
        }

    }
}