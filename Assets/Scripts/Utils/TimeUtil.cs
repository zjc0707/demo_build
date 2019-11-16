using System;
public static class TimeUtil
{
    private static DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    public static long UnixTimeSpan
    {
        get
        {
            return (long)(DateTime.Now - start).TotalSeconds;
        }
    }
}