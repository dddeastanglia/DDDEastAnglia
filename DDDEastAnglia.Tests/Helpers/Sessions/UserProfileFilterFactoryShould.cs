using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers.Sessions;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class UserProfileFilterFactoryShould
    {
        [Test]
        public void CreateTheCorrectFilter_WhenTheAgendaIsNotYetPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(false);
            var sessionRepository = Substitute.For<ISessionRepository>();
            var factory = new UserProfileFilterFactory(sessionRepository);

            var filter = factory.Create(conference);
            Assert.That(filter, Is.InstanceOf<SubmittedSessionProfileFilter>());
        }

        [Test]
        public void CreateTheCorrectFilter_WhenTheAgendaIsPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(true);
            var sessionRepository = Substitute.For<ISessionRepository>();
            var factory = new UserProfileFilterFactory(sessionRepository);

            var filter = factory.Create(conference);
            Assert.That(filter, Is.InstanceOf<SelectedSpeakerProfileFilter>());
        }
    }
}
