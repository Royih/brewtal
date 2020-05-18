using System;
using Brewtal2.BLL;

namespace Brewtal2.Models
{
    public interface ICurrentUser
    {
        Guid? Id { get; }

        string UserName { get; }

        string TenantId { get; }
        bool HasRole(Roles role);
        bool HasAnyRole(Roles[] roles);
    }
}