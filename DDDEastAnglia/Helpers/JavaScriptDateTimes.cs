using System;

namespace DDDEastAnglia.Helpers
{
    public static class JavaScriptDateTimes
    {
        public static long GetJavascriptTimestamp(this DateTime input)
        {
            // taken from https://github.com/flot/flot/blob/master/API.md#time-series-data
            TimeSpan timeSinceEpoch = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            DateTime time = input.Subtract(timeSinceEpoch);
            return time.Ticks / 10000;
        }         
    }
}
