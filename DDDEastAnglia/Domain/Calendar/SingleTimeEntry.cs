using System;

namespace DDDEastAnglia.Domain.Calendar
{
    public class SingleTimeEntry : CalendarEntry
    {
        private readonly DateTimeOffset _startDate;

        public SingleTimeEntry(int calendarItemId, CalendarEntryType entryType, string description, bool isPublic, bool isAuthorised, DateTimeOffset startDate)
            : base(calendarItemId, entryType, description, isPublic, isAuthorised)
        {
            _startDate = startDate;
        }

        public override bool IsOpen()
        {
            return HasPassed();
        }

        public override bool HasPassed()
        {
            return IsAuthorised() && _startDate.UtcDateTime <= DateTime.UtcNow;
        }

        public override bool YetToOpen()
        {
            return !HasPassed();
        }
    }
}
