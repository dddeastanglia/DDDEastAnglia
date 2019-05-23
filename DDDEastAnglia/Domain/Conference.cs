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
        private readonly bool anonymousSessions;
        private readonly Dictionary<CalendarEntryType, CalendarEntry> calendarEntries = new Dictionary<CalendarEntryType, CalendarEntry>();

        public Conference(int id, string name, string shortName, int numberOfTimeSlots = 0, int numberOfTracks = 0, bool anonymousSessions = false)
        {
            this.id = id;
            this.name = name;
            this.shortName = shortName;
            this.numberOfTimeSlots = numberOfTimeSlots;
            this.numberOfTracks = numberOfTracks;
            this.anonymousSessions = anonymousSessions;
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
            return ConferenceTimelineIsActive() && GetCalendarEntry(CalendarEntryType.SessionSubmission).IsOpen();
        }

        public virtual bool CanVote()
        {
            return ConferenceTimelineIsActive() && GetCalendarEntry(CalendarEntryType.Voting).IsOpen();
        }

        public bool AgendaBeingPrepared()
        {
            return calendarEntries[CalendarEntryType.Voting].HasPassed()
                    && calendarEntries[CalendarEntryType.AgendaPublished].YetToOpen();
        }

        public bool CanPublishAgenda()
        {
            return ConferenceTimelineIsActive() && GetCalendarEntry(CalendarEntryType.AgendaPublished).IsOpen();
        }

        public bool CanRegister()
        {
            return ConferenceTimelineIsActive() && GetCalendarEntry(CalendarEntryType.Registration).IsOpen();
        }

        public bool CanShowSessions()
        {
            return CanShowSpeakers();
        }

        public bool CanShowSpeakers()
        {
            return ConferenceTimelineIsActive() && (CanSubmit() || CanVote() || CanPublishAgenda() || CanRegister());
        }

        public bool IsPreview()
        {
            return GetCalendarEntry(CalendarEntryType.Preview).IsOpen();
        }

        public bool IsClosed()
        {
            return GetCalendarEntry(CalendarEntryType.Closed).IsOpen();
        }

        private bool ConferenceTimelineIsActive()
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

        public bool AnonymousSessions()
        {
            return this.AnonymousSessions();
        }
    }
}