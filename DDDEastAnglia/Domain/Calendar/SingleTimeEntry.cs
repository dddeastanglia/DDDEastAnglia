using System;

namespace DDDEastAnglia.Domain.Calendar
{
    public class SingleTimeEntry : CalendarEntry
    {
        private readonly DateTimeOffset startDate;

        public SingleTimeEntry(int calendarItemId, CalendarEntryType entryType, string description, bool isPublic, bool isAuthorised, DateTimeOffset startDate)
            : base(calendarItemId, entryType, description, isPublic, isAuthorised)
        {
            this.startDate = startDate;
        }

        public override bool IsOpen()
        {
            return HasPassed();
        }

        public override bool HasPassed()
        {
            return IsAuthorised() && startDate.UtcDateTime <= DateTime.UtcNow;
        }

        public override bool YetToOpen()
        {
            return !HasPassed();
        }
    }
}
