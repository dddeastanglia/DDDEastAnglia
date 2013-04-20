using System;

namespace DDDEastAnglia.Domain.Calendar
{
    public class TimeRangeEntry : CalendarEntry
    {
        private readonly DateTimeOffset _startDate;
        private readonly DateTimeOffset _endDate;

        public TimeRangeEntry(int id, CalendarEntryType entryType, string description, bool isPublic, bool isAuthorised, DateTimeOffset startDate, DateTimeOffset endDate)
            : base(id, entryType, description, isPublic, isAuthorised)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override bool IsOpen()
        {
            return IsAuthorised() && _startDate.UtcDateTime <= DateTime.UtcNow && DateTime.UtcNow <= _endDate.UtcDateTime;
        }
    }
}