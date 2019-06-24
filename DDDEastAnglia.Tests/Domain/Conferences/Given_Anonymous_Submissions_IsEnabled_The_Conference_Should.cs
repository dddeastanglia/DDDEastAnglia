using DDDEastAnglia.Domain;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Domain.Conferences
{
    [TestFixture]
    public sealed class Given_Anonymous_Submissions_IsEnabled_The_Conference_Should
    {
        [Test]
        public void NotAllowSpeakersToBeShown()
        {
            var conference = new Conference(0, "", "", anonymousSessions: true);
            Assert.IsFalse(conference.CanShowSpeakers());
        }
    }
}
