using System.Collections.Generic;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Domain
{
    public class Conference : IConference
    {
        private readonly Dictionary<CalendarEntryType, CalendarEntry> calendarEntries = new Dictionary<CalendarEntryType, CalendarEntry>();

        public Conference(int id, string name, string shortName, int numberOfTimeSlots = 0, int numberOfTracks = 0)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            NumberOfTimeSlots = numberOfTimeSlots;
            NumberOfTracks = numberOfTracks;
        }

        public int Id { get; }

        public string Name { get; }

        public string ShortName { get; }

        public int NumberOfTimeSlots { get; }

        public int NumberOfTracks { get; }

        public int TotalNumberOfSessions => NumberOfTimeSlots * NumberOfTracks;

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
    }
}