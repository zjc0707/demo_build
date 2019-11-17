using System;
public static class TimeUtil
{
    private static readonly DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    public static long UnixTimeSpan
    {
        get
        {
            return (long)(DateTime.Now - start).TotalSeconds;
        }
    }
    public static string Format(long timeSpan)
    {
        DateTime dateTime = start.AddSeconds((double)timeSpan);
        return dateTime.ToString("yyyy-MM-dd HH:mm");
    }
}