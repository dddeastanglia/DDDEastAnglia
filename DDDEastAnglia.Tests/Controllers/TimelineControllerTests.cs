using System;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class TimelineControllerTests
    {
        [Test]
        public void Details_ShowsThatTheSubmissionPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheSubmissionPeriod()
        {
            // 30 mins before session submission opens
            var controller = SetUpController(CreateDateTime(2010, 3, 31, 23, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.SubmissionPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheSubmissionPeriodHasNotPassed_WhenTheCurrentDateIsDuringTheSubmissionPeriod()
        {
            // 30 mins before session submission closes
            var controller = SetUpController(CreateDateTime(2010, 4, 28, 23, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.SubmissionPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheSubmissionPeriodHasPassed_WhenTheCurrentDateIsAfterTheEndDateForSubmissions()
        {
            // 30 mins after session submission closes
            var controller = SetUpController(CreateDateTime(2010, 4, 29, 0, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.SubmissionPeriodPassed, Is.True);
        }

        [Test]
        public void Details_ShowsThatTheVotingPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheVotingPeriod()
        {
            // 30 mins before session voting opens
            var controller = SetUpController(CreateDateTime(2010, 4, 28, 23, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.VotingPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheVotingPeriodHasNotPassed_WhenTheCurrentDateIsDuringTheVotingPeriod()
        {
            // 30 mins before session voting closes
            var controller = SetUpController(CreateDateTime(2010, 5, 14, 23, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.VotingPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheVotingPeriodHasPassed_WhenTheCurrentDateIsAfterTheEndDateForVoting()
        {
            // 30 mins after session voting closes
            var controller = SetUpController(CreateDateTime(2010, 5, 15, 0, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.VotingPeriodPassed, Is.True);
        }

        [Test]
        public void Details_ShowsThatTheAgendaPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheAgendaBeingPublished()
        {
            // 30 mins before agenda is published
            var controller = SetUpController(CreateDateTime(2010, 5, 25, 8, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.AgendaPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheAgendaPeriodHasNotPassed_WhenTheCurrentDateIsAfterTheAgendaBeingPublished_ButBeforeRegistrationOpening()
        {
            // 30 mins before registraion opens (which signals the end of the agenda period)
            var controller = SetUpController(CreateDateTime(2010, 5, 30, 12, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.AgendaPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheAgendaPeriodHasPassed_WhenTheCurrentDateIsAfterTheStartOfRegistration()
        {
            // 30 mins after registraion opens (which signals the end of the agenda period)
            var controller = SetUpController(CreateDateTime(2010, 5, 30, 13, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.AgendaPeriodPassed, Is.True);
        }

        [Test]
        public void Details_ShowsThatTheRegistrationPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheRegistrationPeriod()
        {
            // 30 mins before registration opens
            var controller = SetUpController(CreateDateTime(2010, 5, 30, 12, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.RegistrationPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheRegistrationPeriodHasNotPassed_WhenTheCurrentDateIsDuringTheRegistrationPeriod()
        {
            // 30 mins aftger registraion opens
            var controller = SetUpController(CreateDateTime(2010, 5, 30, 13, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.RegistrationPeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheRegistrationPeriodHasPassed_WhenTheCurrentDateIsAfterTheEndDateForRegistration()
        {
            // 30 mins after registraion closes
            var controller = SetUpController(CreateDateTime(2010, 6, 5, 22, 30, 0));
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.RegistrationPeriodPassed, Is.True);
        }

        private TimelineController SetUpController(DateTimeOffset currentDateTime)
        {
            var calendarItemRepository = CreateCalendarItemRepository();
            var dateTimeOffsetProvider = new CannedResponseDateTimeOffsetProvider();
            dateTimeOffsetProvider.SetCurrentValue(currentDateTime);

            var controller = new TimelineController(calendarItemRepository, dateTimeOffsetProvider);
            return controller;
        }

        /// <summary>
        /// Creates a calendar item repository that has the following dates:
        /// 
        ///    Session Submission: 01/04/10 00:00:00 - 28/04/10 23:59:59
        ///    Voting:             29/04/10 00:00:00 - 15/05/10 00:00:00
        ///    Agenda Published:   25/05/10 09:00:00
        ///    Registration:       30/05/10 13:00:00 - 05/06/10 22:00:00
        ///    Conference:         06/06/10 09:00:00 - 06/06/10 17:30:00
        /// </summary>
        private ICalendarItemRepository CreateCalendarItemRepository()
        {
            var calendarItemRepository = Substitute.For<ICalendarItemRepository>();

            var sessionSubmission = CreateCalendarItem(CreateDateTime(2010, 4, 1, 0, 0, 0), CreateDateTime(2010, 4, 28, 23, 59, 59));
            calendarItemRepository.GetFromType(CalendarEntryType.SessionSubmission).Returns(sessionSubmission);

            var voting = CreateCalendarItem(CreateDateTime(2010, 4, 29, 0, 0, 0), CreateDateTime(2010, 5, 15, 0, 0, 0));
            calendarItemRepository.GetFromType(CalendarEntryType.Voting).Returns(voting);

            var agendaPublished = CreateCalendarItem(CreateDateTime(2010, 5, 25, 9, 0, 0), null);
            calendarItemRepository.GetFromType(CalendarEntryType.AgendaPublished).Returns(agendaPublished);

            var registration = CreateCalendarItem(CreateDateTime(2010, 5, 30, 13, 0, 0), CreateDateTime(2010, 6, 5, 22, 0, 0));
            calendarItemRepository.GetFromType(CalendarEntryType.Registration).Returns(registration);

            var conference = CreateCalendarItem(CreateDateTime(2010, 6, 6, 9, 0, 0), CreateDateTime(2010, 6, 6, 17, 30, 0));
            calendarItemRepository.GetFromType(CalendarEntryType.Conference).Returns(conference);
            
            return calendarItemRepository;
        }

        private CalendarItem CreateCalendarItem(DateTimeOffset startDate, DateTimeOffset? endDate)
        {
            return new CalendarItem { StartDate = startDate, EndDate = endDate };
        }

        private DateTimeOffset CreateDateTime(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.FromHours(1));
        }
    }
}
