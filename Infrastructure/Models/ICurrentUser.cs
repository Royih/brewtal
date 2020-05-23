using System;

namespace Brewtal2.Infrastructure.Models
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