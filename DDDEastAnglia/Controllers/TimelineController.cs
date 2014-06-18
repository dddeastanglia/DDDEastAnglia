using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class TimelineController : Controller
    {
        private const string DateOnlyPattern = "dddd d MMMM yyyy";
        private const string TimeOnlyPattern = "H:mm";
        private const string DateAndTimePattern = "dddd d MMMM yyyy, H:mm";

        private readonly ICalendarItemRepository calendarItemRepository;
        private readonly IDateTimeOffsetProvider dateTimeOffsetProvider;

        public TimelineController(ICalendarItemRepository calendarItemRepository, IDateTimeOffsetProvider dateTimeOffsetProvider)
        {
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }

            if (dateTimeOffsetProvider == null)
            {
                throw new ArgumentNullException("dateTimeOffsetProvider");
            }
            
            this.calendarItemRepository = calendarItemRepository;
            this.dateTimeOffsetProvider = dateTimeOffsetProvider;
        }

        public ActionResult ConferenceDate()
        {
            var conference = calendarItemRepository.GetFromType(CalendarEntryType.Conference);
            string conferenceDate = conference.StartDate.ToString(DateOnlyPattern);
            return new ContentResult { Content = conferenceDate };
        }

        public ActionResult ConferenceTime()
        {
            var conference = calendarItemRepository.GetFromType(CalendarEntryType.Conference);
            string startTime = conference.StartDate.ToString(TimeOnlyPattern);
            string endTime = conference.EndDate.Value.ToString(TimeOnlyPattern);
            string conferenceTimes = string.Format("{0} to {1}", startTime, endTime);
            return new ContentResult {Content = conferenceTimes};
        }

        public ActionResult Details()
        {
            var sessionSubmission = calendarItemRepository.GetFromType(CalendarEntryType.SessionSubmission);
            var voting = calendarItemRepository.GetFromType(CalendarEntryType.Voting);
            var agendaPublished = calendarItemRepository.GetFromType(CalendarEntryType.AgendaPublished);
            var registraion = calendarItemRepository.GetFromType(CalendarEntryType.Registration);

            var model = new TimelineModel
            {
                SubmissionOpens = FormatStartDate(sessionSubmission.StartDate),
                SubmissionCloses = FormatEndDate(sessionSubmission.EndDate),
                VotingOpens = FormatStartDate(voting.StartDate),
                VotingCloses = FormatEndDate(voting.EndDate),
                AgendaAnnounced = FormatStartDate(agendaPublished.StartDate),
                RegistrationOpens = FormatStartDate(registraion.StartDate),

                SubmissionPeriodPassed = HasDatePassed(sessionSubmission.EndDate.Value),
                VotingPeriodPassed = HasDatePassed(voting.EndDate.Value),
                AgendaPeriodPassed = HasDatePassed(registraion.StartDate),
                RegistrationPeriodPassed = HasDatePassed(registraion.EndDate.Value)
            };
            
            return PartialView("_Timeline", model);
        }

        private bool HasDatePassed(DateTimeOffset dateTime)
        {
            return dateTimeOffsetProvider.CurrentDateTime() > dateTime;
        }

        private string FormatStartDate(DateTimeOffset startDateTime)
        {
            var timeOfDay = startDateTime.TimeOfDay;
            return timeOfDay.Hours == 0
                        ? startDateTime.ToString(DateOnlyPattern)
                        : startDateTime.ToString(DateAndTimePattern);
        }

        private string FormatEndDate(DateTimeOffset? endDateTime)
        {
            if (endDateTime == null)
            {
                return string.Empty;
            }

            var endDateTimeValue = endDateTime.Value;
            var timeOfDay = endDateTimeValue.TimeOfDay;

            // if the end time is midnight, we need to subtract a little to push it back to the previous date
            if (timeOfDay.Hours == 0 && timeOfDay.Minutes == 0 && timeOfDay.Seconds == 0)
            {
                endDateTimeValue = endDateTimeValue - new TimeSpan(0, 0, 1);
            }

            return endDateTimeValue.ToString(DateOnlyPattern);
        }
    }
}
