using System;

namespace DDDEastAnglia.Helpers
{
    public interface ITimeProvider
    {
        DateTime UtcNow { get; }
    }

    public class TimeProvider : ITimeProvider
    {
        public DateTime UtcNow { get { return DateTime.UtcNow; } }
    }
}