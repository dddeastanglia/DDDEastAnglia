using System.Collections.Generic;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Domain
{
    public class Conference : IConference
    {
        private readonly int id;
        private readonly string name;
        private readonly string shortName;
        private readonly Dictionary<CalendarEntryType, CalendarEntry> calendarEntries = new Dictionary<CalendarEntryType, CalendarEntry>();

        public Conference(int id, string name, string shortName)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
        }

        public int Id
        {
            get { return id; }
        }

        public bool CanSubmit()
        {
            return IsNotInSpecialMode() && GetCalendarEntry(CalendarEntryType.SessionSubmission).IsOpen();
        }

        public virtual bool CanVote()
        {
            return IsNotInSpecialMode() && GetCalendarEntry(CalendarEntryType.Voting).IsOpen();
        }

        public bool CanPublishAgenda()
        {
            return IsNotInSpecialMode() && GetCalendarEntry(CalendarEntryType.AgendaPublished).IsOpen();
        }

        public bool CanRegister()
        {
            return IsNotInSpecialMode() && GetCalendarEntry(CalendarEntryType.Registration).IsOpen();
        }

        public bool CanShowSessions()
        {
            return CanShowSpeakers();
        }

        public bool CanShowSpeakers()
        {
            return IsNotInSpecialMode() && (CanSubmit() || CanVote() || CanPublishAgenda() || CanRegister());
        }

        public bool IsPreview()
        {
            return GetCalendarEntry(CalendarEntryType.Preview).IsOpen();
        }

        public bool IsClosed()
        {
            return GetCalendarEntry(CalendarEntryType.Closed).IsOpen();
        }

        private bool IsNotInSpecialMode()
        {
            return !IsPreview() && !IsClosed();
        }

        public void AddToCalendar(CalendarEntry entry)
        {
            var calendarEntryType = entry.GetEntryType();
            
            if (calendarEntryType != CalendarEntryType.Unknown)
            {
                calendarEntries[calendarEntryType] = entry;
            }
        }

        private CalendarEntry GetCalendarEntry(CalendarEntryType calendarEntryType)
        {
            CalendarEntry calendarEntry;
            return calendarEntries.TryGetValue(calendarEntryType, out calendarEntry)
                ? calendarEntry
                : new NullCalendarEntry();
        }
    }
}