using System;

namespace DDDEastAnglia.Domain.Calendar
{
    public class TimeRangeEntry : CalendarEntry
    {
        private readonly DateTimeOffset startDate;
        private readonly DateTimeOffset endDate;

        public TimeRangeEntry(CalendarEntryType entryType, bool isAuthorised, DateTimeOffset startDate, DateTimeOffset endDate)
            : base(entryType, isAuthorised)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public override bool IsOpen()
        {
            return IsAuthorised() && startDate.UtcDateTime <= DateTime.UtcNow && DateTime.UtcNow <= endDate.UtcDateTime;
        }

        public override bool HasPassed()
        {
            return IsAuthorised() && endDate.UtcDateTime <= DateTime.UtcNow;
        }

        public override bool YetToOpen()
        {
            return IsAuthorised() && startDate > DateTime.UtcNow;
        }
    }
}
