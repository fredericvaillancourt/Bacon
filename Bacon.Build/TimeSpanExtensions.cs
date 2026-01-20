namespace Bacon.Build;

public static class TimeSpanExtensions
{
    public static string ToShortString(this TimeSpan timeSpan)
    {
        if (timeSpan < TimeSpan.FromSeconds(1))
        {
            return "< 1 sec";
        }

        if (timeSpan < TimeSpan.FromMinutes(1))
        {
            return timeSpan.Seconds != 1 ? $"{timeSpan.Seconds} secs" : "1 sec";
        }

        if (timeSpan < TimeSpan.FromHours(1))
        {
            return $"{timeSpan:m\\:ss}";
        }

        if (timeSpan < TimeSpan.FromDays(1))
        {
            return $"{timeSpan:h\\:mm\\:ss}";
        }

        return $"{timeSpan:d\\.hh\\:mm\\:ss}";
    }
}