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
            calendarEntries.Add(CalendarEntryType.SessionSubmission, new NullCalendarEntry());
            calendarEntries.Add(CalendarEntryType.Voting, new NullCalendarEntry());
            calendarEntries.Add(CalendarEntryType.AgendaPublished, new NullCalendarEntry());
            calendarEntries.Add(CalendarEntryType.Registration, new NullCalendarEntry());
            calendarEntries.Add(CalendarEntryType.Conference, new NullCalendarEntry());
        }

        public int Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
        }

        public string ShortName
        {
            get { return shortName; }
        }

        public bool CanSubmit()
        {
            return calendarEntries[CalendarEntryType.SessionSubmission].IsOpen();
        }

        public virtual bool CanVote()
        {
            return calendarEntries[CalendarEntryType.Voting].IsOpen();
        }

        public bool CanPublishAgenda()
        {
            return calendarEntries[CalendarEntryType.AgendaPublished].IsOpen();
        }

        public bool CanRegister()
        {
            return calendarEntries[CalendarEntryType.Registration].IsOpen();
        }

        public bool CanShowSessions()
        {
            return CanSubmit() || CanVote() || CanPublishAgenda() || CanRegister();
        }

        public bool CanShowSpeakers()
        {
            return CanSubmit() || CanVote() || CanPublishAgenda() || CanRegister();
        }

        public void AddToCalendar(CalendarEntry entry)
        {
            var calendarEntryType = entry.GetEntryType();
            
            if (calendarEntryType != CalendarEntryType.Unknown)
            {
                calendarEntries[calendarEntryType] = entry;
            }
        }
    }
}