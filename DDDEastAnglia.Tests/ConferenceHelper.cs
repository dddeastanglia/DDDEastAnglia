using System;
using DDDEastAnglia.Domain.Calendar;

namespace DDDEastAnglia.Tests
{
    public class ConferenceHelper
    {
        public static TimeRangeEntry GetOpenVotingPeriod()
        {
            return GetTimeRangeEntry(CalendarEntryType.Voting, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(1));
        }

        public static CalendarEntry GetPastVotingPeriod()
        {
            return GetTimeRangeEntry(CalendarEntryType.Voting, DateTimeOffset.Now.AddDays(-2), DateTime.Now.AddDays(-1));
        }

        public static CalendarEntry GetFutureVotingPeriod()
        {
            return GetTimeRangeEntry(CalendarEntryType.Voting, DateTimeOffset.Now.AddDays(1), DateTime.Now.AddDays(2));
        }

        public static CalendarEntry GetOpenSubmissionPeriod()
        {
            return GetTimeRangeEntry(CalendarEntryType.SessionSubmission, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(1));
        }

        public static CalendarEntry GetPastSubmissionPeriod()
        {
            return GetTimeRangeEntry(CalendarEntryType.SessionSubmission, DateTimeOffset.Now.AddDays(-2), DateTime.Now.AddDays(-1));
        }

        public static CalendarEntry GetFutureAgendaPublishingDate()
        {
            return GetSingleTimeEntry(CalendarEntryType.AgendaPublished, DateTimeOffset.Now.AddDays(1));
        }

        public static CalendarEntry GetOpenAgenda()
        {
            return GetSingleTimeEntry(CalendarEntryType.AgendaPublished, DateTimeOffset.Now.AddDays(-1));
        }

        public static CalendarEntry GetFutureRegistration()
        {
            return GetTimeRangeEntry(CalendarEntryType.Registration, DateTimeOffset.Now.AddDays(1), DateTime.Now.AddDays(2));
        }

        public static CalendarEntry GetOpenRegistration()
        {
            return GetTimeRangeEntry(CalendarEntryType.Registration, DateTimeOffset.Now.AddDays(-1), DateTimeOffset.Now.AddDays(1));
        }

        private static SingleTimeEntry GetSingleTimeEntry(CalendarEntryType calendarEntryType, DateTimeOffset startTime)
        {
            return new SingleTimeEntry(calendarEntryType, true, startTime);
        }

        private static TimeRangeEntry GetTimeRangeEntry(CalendarEntryType calendarEntryType, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return new TimeRangeEntry(calendarEntryType,
                                      true,
                                      startDate,
                                      endDate);
        }
    }
}