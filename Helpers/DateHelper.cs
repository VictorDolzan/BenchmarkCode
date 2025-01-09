namespace BenchmarkCode.Helpers;

public static class DateHelper
{
   public static DateTime GetBrasiliaTime()
    {
        DateTime utcNow = DateTime.UtcNow;

        TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        return TimeZoneInfo.ConvertTimeFromUtc(utcNow, brasiliaTimeZone);
    }
}