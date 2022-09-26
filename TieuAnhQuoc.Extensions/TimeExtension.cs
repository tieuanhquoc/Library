namespace TieuAnhQuoc.Extensions;

public static class TimeExtension
{
    private static readonly string LocalTimeZone = Environment.GetEnvironmentVariable("TZ") ?? "Asia/Ho_Chi_Minh";

    public static long NowSeconds()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public static long TotalSeconds(this DateTime dateTime)
    {
        var dateTimeOffset = new DateTimeOffset(dateTime);
        return dateTimeOffset.ToUnixTimeSeconds();
    }

    public static DateTime Now()
    {
        return DateTime.UtcNow;
    }

    public static DateTimeOffset Now(string timeZoneId)
    {
        return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTimeOffset.UtcNow, timeZoneId);
    }

    public static DateTime ToLocal(this DateTime dateTime, string convertToTimeZone)
    {
        if (convertToTimeZone.IsNullOrEmpty())
            convertToTimeZone = LocalTimeZone;

        if (dateTime.Kind == DateTimeKind.Local)
            return dateTime;

        var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(convertToTimeZone);
        return TimeZoneInfo.ConvertTimeFromUtc(dateTime, sourceTimeZone);
    }

    public static DateTime ToUtc(this DateTime dateTime, string sourceTimeZoneId)
    {
        if (sourceTimeZoneId.IsNullOrEmpty())
            sourceTimeZoneId = LocalTimeZone;
        if (dateTime.Kind == DateTimeKind.Utc)
        {
            return dateTime;
        }

        var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
        return TimeZoneInfo.ConvertTimeToUtc(dateTime, sourceTimeZone);
    }
}