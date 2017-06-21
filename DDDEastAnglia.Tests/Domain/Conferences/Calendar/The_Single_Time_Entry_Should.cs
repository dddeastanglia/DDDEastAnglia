using System;
using DDDEastAnglia.Domain.Calendar;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Domain.Conferences.Calendar
{
    [TestFixture]
    public class The_Single_Time_Entry_Should
    {
        [Test]
        public void Be_Closed_If_Authorised_But_Set_In_The_Future()
        {
            Given_An_Authorised_SingleTimeEntry_In_The_Future();
            Then_It_Is_Closed();
        }

        [Test]
        public void Be_Open_If_Authorised_And_Started_In_The_Past()
        {
            Given_An_Authorised_SingleTimeEntry_In_The_Past();
            Then_It_Is_Open();
        }

        [Test]
        public void Be_Closed_If_Not_Authorised_And_In_The_Future()
        {
            Given_An_Unauthorised_SingleTimeEntry_In_The_Future();
            Then_It_Is_Closed();
        }

        [Test]
        public void Be_Closed_If_Not_Authorised_And_In_The_Past()
        {
            Given_An_Unauthorised_SingleTimeEntry_In_The_Past();
            Then_It_Is_Closed();
        }

        private void Given_An_Authorised_SingleTimeEntry_In_The_Future()
        {
            _singleTimeEntry = new SingleTimeEntry(CalendarEntryType.AgendaPublished, true, DateTimeOffset.Now.AddDays(1));
        }

        private void Given_An_Authorised_SingleTimeEntry_In_The_Past()
        {
            _singleTimeEntry = new SingleTimeEntry(CalendarEntryType.AgendaPublished, true, DateTimeOffset.Now.AddDays(-1));
        }

        private void Given_An_Unauthorised_SingleTimeEntry_In_The_Future()
        {
            _singleTimeEntry = new SingleTimeEntry(CalendarEntryType.AgendaPublished, false, DateTimeOffset.Now.AddDays(1));
        }

        private void Given_An_Unauthorised_SingleTimeEntry_In_The_Past()
        {
            _singleTimeEntry = new SingleTimeEntry(CalendarEntryType.AgendaPublished, false, DateTimeOffset.Now.AddDays(-1));
        }

        private void Then_It_Is_Closed()
        {
            Assert.That(_singleTimeEntry.IsOpen(), Is.False);
        }

        private void Then_It_Is_Open()
        {
            Assert.That(_singleTimeEntry.IsOpen(), Is.True);
        }


        private SingleTimeEntry _singleTimeEntry;
    }
}