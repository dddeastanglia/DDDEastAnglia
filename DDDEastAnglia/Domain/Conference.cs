using System.Collections.Generic;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Domain
{
    public class Conference
    {
        private readonly int _id;
        private readonly string _name;
        private readonly string _shortName;
        private readonly Dictionary<CalendarEntryType, CalendarEntry> _calendarEntries = new Dictionary<CalendarEntryType, CalendarEntry>();

        public Conference(int id, string name, string shortName)
        {
            _id = id;
            _name = name;
            _shortName = shortName;
            _calendarEntries.Add(CalendarEntryType.SessionSubmission, new NullCalendarEntry());
            _calendarEntries.Add(CalendarEntryType.Voting, new NullCalendarEntry());
            _calendarEntries.Add(CalendarEntryType.AgendaPublished, new NullCalendarEntry());
            _calendarEntries.Add(CalendarEntryType.Registration, new NullCalendarEntry());
            _calendarEntries.Add(CalendarEntryType.Conference, new NullCalendarEntry());
        }

        public int Id
        {
            get { return _id; }
        }

        public bool CanSubmit()
        {
            return _calendarEntries[CalendarEntryType.SessionSubmission].IsOpen();
        }

        public bool CanVote()
        {
            return _calendarEntries[CalendarEntryType.Voting].IsOpen();
        }

        public bool CanPublishAgenda()
        {
            return _calendarEntries[CalendarEntryType.AgendaPublished].IsOpen();
        }

        public bool CanRegister()
        {
            return _calendarEntries[CalendarEntryType.Registration].IsOpen();
        }

        public void AddToCalendar(CalendarEntry entry)
        {
            var calendarEntryType = entry.GetEntryType();
            if (calendarEntryType == CalendarEntryType.Unknown)
            {
                return;
            }
            _calendarEntries[calendarEntryType] = entry;
        }
    }
}