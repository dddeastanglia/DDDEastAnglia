using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers.Sessions;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class SessionLoaderFactoryShould
    {
        [Test]
        public void CreateTheCorrectLoader_WhenTheAgendaIsNotYetPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(false);
            var sessionRepository = Substitute.For<ISessionRepository>();
            var factory = new SessionLoaderFactory(sessionRepository);

            var filter = factory.Create(conference);
            Assert.That(filter, Is.InstanceOf<AllSessionsLoader>());
        }

        [Test]
        public void CreateTheCorrectLoader_WhenTheAgendaIsPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(true);
            var sessionRepository = Substitute.For<ISessionRepository>();
            var factory = new SessionLoaderFactory(sessionRepository);

            var filter = factory.Create(conference);
            Assert.That(filter, Is.InstanceOf<SelectedSessionsLoader>());
        }
    }
}
