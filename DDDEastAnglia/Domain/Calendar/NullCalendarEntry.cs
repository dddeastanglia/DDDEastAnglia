namespace DDDEastAnglia.Domain.Calendar
{
    public class NullCalendarEntry : CalendarEntry
    {
        public NullCalendarEntry()
            : base(CalendarEntryType.Unknown, false)
        {}

        public override bool IsOpen()
        {
            return false;
        }

        public override bool HasPassed()
        {
            return false;
        }

        public override bool YetToOpen()
        {
            return false;
        }
    }
}
