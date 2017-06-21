using System;
using DDDEastAnglia.Domain.Calendar;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Domain.Conferences.Calendar
{
    [TestFixture]
    public class The_TimeRangeEntry_Should
    {
        [Test]
        public void Be_Closed_If_Not_Authorised_And_The_Window_Has_Passed()
        {
            Given_An_Unauthorised_TimeRangeEntry_In_The_Past();
            Then_It_Should_Be_Closed();
        }

        [Test]
        public void Be_Closed_If_Not_Authorised_In_The_Window()
        {
            Given_An_Unauthorised_TimeRangeEntry_But_Is_In_The_Window();
            Then_It_Should_Be_Closed();
        }

        [Test]
        public void Be_Closed_If_Not_Authorised_And_In_The_Future()
        {
            Given_An_Unauthorised_TimeRangeEntry_And_Is_In_The_Future();
            Then_It_Should_Be_Closed();
        }

        [Test]
        public void Be_Closed_If_Authorised_But_The_Window_Has_Passed()
        {
            Given_An_Authorised_TimeRangeEntry_In_The_Past();
            Then_It_Should_Be_Closed();
        }

        [Test]
        public void Be_Open_If_Authorised_And_Is_In_The_Window()
        {
            Given_An_Authorised_TimeRangeEntry_But_Is_In_The_Window();
            Then_It_Should_Be_Open();
        }

        [Test]
        public void Be_Closed_If_Authorised_And_In_The_Future()
        {
            Given_An_Authorised_TimeRangeEntry_And_Is_In_The_Future();
            Then_It_Should_Be_Closed();
        }

        private void Given_An_Authorised_TimeRangeEntry_In_The_Past()
        {
            _timeRangeEntry = new TimeRangeEntry(CalendarEntryType.Conference, true, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1));
        }

        private void Given_An_Unauthorised_TimeRangeEntry_In_The_Past()
        {
            _timeRangeEntry = new TimeRangeEntry(CalendarEntryType.Conference, false, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1));
        }

        private void Given_An_Unauthorised_TimeRangeEntry_But_Is_In_The_Window()
        {
            _timeRangeEntry = new TimeRangeEntry(CalendarEntryType.Conference, false, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(2));
        }

        private void Given_An_Unauthorised_TimeRangeEntry_And_Is_In_The_Future()
        {
            _timeRangeEntry = new TimeRangeEntry(CalendarEntryType.Conference, false, DateTimeOffset.Now.AddDays(1), DateTimeOffset.Now.AddDays(2));
        }

        private void Given_An_Authorised_TimeRangeEntry_But_Is_In_The_Window()
        {
            _timeRangeEntry = new TimeRangeEntry(CalendarEntryType.Conference, true, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(2));
        }

        private void Given_An_Authorised_TimeRangeEntry_And_Is_In_The_Future()
        {
            _timeRangeEntry = new TimeRangeEntry(CalendarEntryType.Conference, true, DateTimeOffset.Now.AddDays(1), DateTimeOffset.Now.AddDays(2));
        }

        private void Then_It_Should_Be_Closed()
        {
            Assert.That(_timeRangeEntry.IsOpen(), Is.False);
        }

        private void Then_It_Should_Be_Open()
        {
            Assert.That(_timeRangeEntry.IsOpen(), Is.True);
        }

        private TimeRangeEntry _timeRangeEntry;
    }
}