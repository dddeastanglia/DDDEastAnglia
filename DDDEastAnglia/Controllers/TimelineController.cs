using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class TimelineController : Controller
    {

        private readonly ICalendarItemRepository calendarItemRepository;
        private readonly IDateTimeFormatter dateTimeFormatter;
        private readonly IDateTimePassedEvaluator dateTimePassedEvaluator;

        public TimelineController(ICalendarItemRepository calendarItemRepository, IDateTimeFormatter dateTimeFormatter, IDateTimePassedEvaluator dateTimePassedEvaluator)
        {
            if (calendarItemRepository == null)
            {
                throw new ArgumentNullException("calendarItemRepository");
            }

            if (dateTimeFormatter == null)
            {
                throw new ArgumentNullException("dateTimeFormatter");
            }

            if (dateTimePassedEvaluator == null)
            {
                throw new ArgumentNullException("dateTimePassedEvaluator");
            }
            
            this.calendarItemRepository = calendarItemRepository;
            this.dateTimeFormatter = dateTimeFormatter;
            this.dateTimePassedEvaluator = dateTimePassedEvaluator;
        }

        public ActionResult ConferenceDate()
        {
            var conference = calendarItemRepository.GetFromType(CalendarEntryType.Conference);
            string conferenceDate = dateTimeFormatter.FormatDate(conference.StartDate);
            return new ContentResult {Content = conferenceDate};
        }

        public ActionResult ConferenceTime()
        {
            var conference = calendarItemRepository.GetFromType(CalendarEntryType.Conference);
            string startTime = dateTimeFormatter.FormatTime(conference.StartDate);
            string endTime = dateTimeFormatter.FormatTime(conference.EndDate.Value);
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
                SessionSubmissionOpens = new TimelineItemModel
                {
                    PeriodDate = dateTimeFormatter.FormatStartDate(sessionSubmission.StartDate),
                    PeriodPassed = dateTimePassedEvaluator.HasDatePassed(sessionSubmission.EndDate.Value)
                },
                SessionSubmissionCloses = new TimelineItemModel
                {
                    PeriodDate = dateTimeFormatter.FormatEndDate(sessionSubmission.EndDate),
                    PeriodPassed = dateTimePassedEvaluator.HasDatePassed(sessionSubmission.EndDate.Value)
                },
                VotingOpens = new TimelineItemModel
                {
                    PeriodDate = dateTimeFormatter.FormatStartDate(voting.StartDate),
                    PeriodPassed = dateTimePassedEvaluator.HasDatePassed(voting.EndDate.Value)
                },
                VotingCloses = new TimelineItemModel
                {
                    PeriodDate = dateTimeFormatter.FormatEndDate(voting.EndDate),
                    PeriodPassed = dateTimePassedEvaluator.HasDatePassed(voting.EndDate.Value)
                },
                AgendaAnnounced = new TimelineItemModel
                {
                    PeriodDate = dateTimeFormatter.FormatStartDate(agendaPublished.StartDate),
                    PeriodPassed = dateTimePassedEvaluator.HasDatePassed(registraion.StartDate)
                },
                RegistrationOpens = new TimelineItemModel
                {
                    PeriodDate = dateTimeFormatter.FormatStartDate(registraion.StartDate),
                    PeriodPassed = dateTimePassedEvaluator.HasDatePassed(registraion.EndDate.Value)
                }
            };
            
            return PartialView("_Timeline", model);
        }
    }
}
