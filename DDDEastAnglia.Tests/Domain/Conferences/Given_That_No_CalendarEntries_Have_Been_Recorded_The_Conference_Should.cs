using DDDEastAnglia.Domain;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Domain.Conferences
{
    [TestFixture]
    public class Given_That_No_CalendarEntries_Have_Been_Recorded_The_Conference_Should
    {
        [Test]
        public void Not_Be_Open_For_Session_Submission()
        {
            Given_That_The_Conference_Has_No_CalendarEntries_Recorded();
            Then_The_Conference_Is_Closed_To_Session_Submissions();
        }

        [Test]
        public void Not_Be_Open_For_Voting()
        {
            Given_That_The_Conference_Has_No_CalendarEntries_Recorded();
            Then_The_Conference_Is_Closed_To_Voting();
        }

        [Test]
        public void Not_Be_Open_For_AgendaPublishing()
        {
            Given_That_The_Conference_Has_No_CalendarEntries_Recorded();
            Then_The_Conference_Is_Closed_To_AgendaPublishing();
        }

        [Test]
        public void Not_Be_Open_For_Registration()
        {
            Given_That_The_Conference_Has_No_CalendarEntries_Recorded();
            Then_The_Conference_Is_Closed_To_Registration();
        }

        private void Given_That_The_Conference_Has_No_CalendarEntries_Recorded()
        {
            _conference = new Conference(1, "", "");
        }

        private void Then_The_Conference_Is_Closed_To_Session_Submissions()
        {
            Assert.That(_conference.CanSubmit(), Is.False);
        }

        private void Then_The_Conference_Is_Closed_To_Voting()
        {
            Assert.That(_conference.CanVote(), Is.False);
        }

        private void Then_The_Conference_Is_Closed_To_AgendaPublishing()
        {
            Assert.That(_conference.CanPublishAgenda(), Is.False);
        }

        private void Then_The_Conference_Is_Closed_To_Registration()
        {
            Assert.That(_conference.CanRegister(), Is.False);
        }

        private Conference _conference;
    }
}