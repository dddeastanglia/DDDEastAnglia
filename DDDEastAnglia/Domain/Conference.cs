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
            return !IsPreview() && GetCalendarEntry(CalendarEntryType.SessionSubmission).IsOpen();
        }

        public virtual bool CanVote()
        {
            return !IsPreview() && GetCalendarEntry(CalendarEntryType.Voting).IsOpen();
        }

        public bool CanPublishAgenda()
        {
            return !IsPreview() && GetCalendarEntry(CalendarEntryType.AgendaPublished).IsOpen();
        }

        public bool CanRegister()
        {
            return !IsPreview() && GetCalendarEntry(CalendarEntryType.Registration).IsOpen();
        }

        public bool CanShowSessions()
        {
            return !IsPreview() && (CanSubmit() || CanVote() || CanPublishAgenda() || CanRegister());
        }

        public bool CanShowSpeakers()
        {
            return !IsPreview() && (CanSubmit() || CanVote() || CanPublishAgenda() || CanRegister());
        }

        public bool IsPreview()
        {
            return GetCalendarEntry(CalendarEntryType.Preview).IsOpen();
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