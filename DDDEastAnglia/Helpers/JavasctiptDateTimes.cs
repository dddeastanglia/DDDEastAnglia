using System;

namespace DDDEastAnglia.Helpers
{
    public static class JavasctiptDateTimes
    {
        public static long GetJavascriptTimestamp(this DateTime input)
        {
            TimeSpan span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            DateTime time = input.Subtract(span);
            return time.Ticks / 10000;
        }         
    }
}
