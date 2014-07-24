namespace DDDEastAnglia.Domain.Calendar
{
    public class NullCalendarEntry : CalendarEntry
    {
        public NullCalendarEntry()
            : base(0, CalendarEntryType.Unknown, "", false, false)
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
