using System;

namespace brewtal.Extensions
{
    public static class Extensions
    {
        public static DateTime SpecifyUtcTime(this DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second, DateTimeKind.Utc);
        }
        public static DateTime? SpecifyUtcTime(this DateTime? input)
        {
            if (!input.HasValue)
                return null;
            return SpecifyUtcTime(input.Value);
        }
    }
}