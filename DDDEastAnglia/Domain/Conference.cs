using System.Collections.Generic;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Domain
{
    public class Conference : IConference
    {
        private readonly int id;
        private readonly string name;
        private readonly string shortName;
        private readonly int numberOfTimeSlots;
        private readonly int numberOfTracks;
        private readonly Dictionary<CalendarEntryType, CalendarEntry> calendarEntries = new Dictionary<CalendarEntryType, CalendarEntry>();

        public Conference(int id, string name, string shortName, int numberOfTimeSlots = 0, int numberOfTracks = 0)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.numberOfTimeSlots = numberOfTimeSlots;
            this.numberOfTracks = numberOfTracks;
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

        public int NumberOfTimeSlots
        {
            get { return numberOfTimeSlots; }
        }

        public int NumberOfTracks
        {
            get { return numberOfTracks; }
        }

        public int TotalNumberOfSessions { get { return NumberOfTimeSlots * NumberOfTracks; } }

        public bool CanSubmit()
        {
            return calendarEntries[CalendarEntryType.SessionSubmission].IsOpen();
        }

        public virtual bool CanVote()
        {
            return calendarEntries[CalendarEntryType.Voting].IsOpen();
        }

        public bool AgendaBeingPrepared()
        {
            return calendarEntries[CalendarEntryType.Voting].HasPassed()
                    && calendarEntries[CalendarEntryType.AgendaPublished].YetToOpen();
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