namespace DDDEastAnglia.Domain.Calendar
{
    public abstract class CalendarEntry
    {
        private readonly CalendarEntryType entryType;
        private readonly bool isAuthorised;

        protected CalendarEntry(CalendarEntryType entryType, bool isAuthorised)
        {
            this.entryType = entryType;
            this.isAuthorised = isAuthorised;
        }

        public bool IsAuthorised()
        {
            return isAuthorised;
        }

        public CalendarEntryType GetEntryType()
        {
            return entryType;
        }

        public abstract bool IsOpen();
        public abstract bool HasPassed();
        public abstract bool YetToOpen();
    }
}
