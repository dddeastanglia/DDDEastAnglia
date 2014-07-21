namespace DDDEastAnglia.Domain.Calendar
{
    public abstract class CalendarEntry
    {
        private readonly int id;
        private readonly CalendarEntryType entryType;
        private readonly string description;
        private readonly bool isPublic;
        private readonly bool isAuthorised;

        protected CalendarEntry(int id, CalendarEntryType entryType, string description, bool isPublic, bool isAuthorised)
        {
            this.id = id;
            this.entryType = entryType;
            this.description = description;
            this.isPublic = isPublic;
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
