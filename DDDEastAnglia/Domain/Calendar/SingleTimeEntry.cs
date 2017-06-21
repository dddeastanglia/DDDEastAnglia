using System;

namespace DDDEastAnglia.Domain.Calendar
{
    public class SingleTimeEntry : CalendarEntry
    {
        private readonly DateTimeOffset startDate;

        public SingleTimeEntry(CalendarEntryType entryType, bool isAuthorised, DateTimeOffset startDate)
            : base(entryType, isAuthorised)
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
