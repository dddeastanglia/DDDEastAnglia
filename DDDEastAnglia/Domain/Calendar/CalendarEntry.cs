namespace DDDEastAnglia.Domain.Calendar
{
    public abstract class CalendarEntry
    {
        private readonly int _id;
        private readonly CalendarEntryType _entryType;
        private readonly string _description;
        private readonly bool _isPublic;
        private readonly bool _isAuthorised;

        protected CalendarEntry(int id, CalendarEntryType entryType, string description, bool isPublic, bool isAuthorised)
        {
            _id = id;
            _entryType = entryType;
            _description = description;
            _isPublic = isPublic;
            _isAuthorised = isAuthorised;
        }

        public bool IsAuthorised()
        {
            return _isAuthorised;
        }

        public CalendarEntryType GetEntryType()
        {
            return _entryType;
        }

        public abstract bool IsOpen();
    }
}