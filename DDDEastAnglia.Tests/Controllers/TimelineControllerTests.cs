using System;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Helpers;
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
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.SessionSubmissionOpens.PeriodPassed, Is.False);
            Assert.That(viewModel.SessionSubmissionCloses.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheSubmissionPeriodHasNotPassed_WhenTheCurrentDateIsDuringTheSubmissionPeriod()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.SessionSubmissionOpens.PeriodPassed, Is.False);
            Assert.That(viewModel.SessionSubmissionCloses.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheSubmissionPeriodHasPassed_WhenTheCurrentDateIsAfterTheEndDateForSubmissions()
        {
            var controller = SetUpController(PeriodPassed.Yes);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.SessionSubmissionOpens.PeriodPassed, Is.True);
            Assert.That(viewModel.SessionSubmissionCloses.PeriodPassed, Is.True);
        }

        [Test]
        public void Details_ShowsThatTheVotingPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheVotingPeriod()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.VotingOpens.PeriodPassed, Is.False);
            Assert.That(viewModel.VotingCloses.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheVotingPeriodHasNotPassed_WhenTheCurrentDateIsDuringTheVotingPeriod()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.VotingOpens.PeriodPassed, Is.False);
            Assert.That(viewModel.VotingCloses.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheVotingPeriodHasPassed_WhenTheCurrentDateIsAfterTheEndDateForVoting()
        {
            var controller = SetUpController(PeriodPassed.Yes);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.VotingOpens.PeriodPassed, Is.True);
            Assert.That(viewModel.VotingCloses.PeriodPassed, Is.True);
        }

        [Test]
        public void Details_ShowsThatTheAgendaPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheAgendaBeingPublished()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.AgendaAnnounced.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheAgendaPeriodHasNotPassed_WhenTheCurrentDateIsAfterTheAgendaBeingPublished_ButBeforeRegistrationOpening()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.AgendaAnnounced.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheAgendaPeriodHasPassed_WhenTheCurrentDateIsAfterTheStartOfRegistration()
        {
            var controller = SetUpController(PeriodPassed.Yes);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.AgendaAnnounced.PeriodPassed, Is.True);
        }

        [Test]
        public void Details_ShowsThatTheRegistrationPeriodHasNotPassed_WhenTheCurrentDateIsBeforeTheRegistrationPeriod()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.RegistrationOpens.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheRegistrationPeriodHasNotPassed_WhenTheCurrentDateIsDuringTheRegistrationPeriod()
        {
            var controller = SetUpController(PeriodPassed.No);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.RegistrationOpens.PeriodPassed, Is.False);
        }

        [Test]
        public void Details_ShowsThatTheRegistrationPeriodHasPassed_WhenTheCurrentDateIsAfterTheEndDateForRegistration()
        {
            var controller = SetUpController(PeriodPassed.Yes);
            var viewModel = controller.Details().GetViewModel<TimelineModel>();

            Assert.That(viewModel.RegistrationOpens.PeriodPassed, Is.True);
        }

        private TimelineController SetUpController(PeriodPassed periodPassed)
        {
            var start = new DateTimeOffset(DateTime.Now, TimeSpan.FromHours(1));
            var end = start + TimeSpan.FromDays(1);
            var calendarItem = new CalendarItem { StartDate = start, EndDate = end };
            var calendarItemRepository = Substitute.For<ICalendarItemRepository>();
            calendarItemRepository.GetFromType(Arg.Any<CalendarEntryType>()).Returns(calendarItem);

            var dateTimeFormatter = Substitute.For<IDateTimeFormatter>();
            var dateTimePassedEvaluator = Substitute.For<IDateTimePassedEvaluator>();
            dateTimePassedEvaluator.HasDatePassed(Arg.Any<DateTimeOffset>()).Returns(periodPassed == PeriodPassed.Yes);

            var controller = new TimelineController(calendarItemRepository, dateTimeFormatter, dateTimePassedEvaluator);
            return controller;
        }

        private enum PeriodPassed
        {
            Yes,
            No
        }
    }
}
