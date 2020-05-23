using System.Collections.Generic;
using System.Linq;
using Brewtal2.Infrastructure.Models;

namespace Brewtal2.Infrastructure
{

    public enum Roles
    {
        User,
        Admin
    }
    public static class Constants
    {
        public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789æøåÆØÅ-@. _";
        public const string DefaultDeviceGroupTitle = "todo: Name me";
        public const string ConfigQueueNameTo = "QueueNameTo";
        public const string ConfigQueueNameFrom = "QueueNameFrom";
        public const string ConfigQueueConnection = "QueueConnection";
        public const string TenantIdHeaderKey = "tenantId";

        public static IEnumerable<CultureDto> ListCultures
        {
            get
            {
                yield return new CultureDto { Key = "nb-NO", Value = "Norsk" };
                yield return new CultureDto { Key = "en-US", Value = "English" };
            }
        }
        public static Models.CultureDto GetCulture(string key)
        {
            return ListCultures.Single(x => x.Key == key);
        }
    }
}