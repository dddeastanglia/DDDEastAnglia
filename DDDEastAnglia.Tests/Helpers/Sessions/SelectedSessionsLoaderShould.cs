using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class SelectedSessionsLoaderShould
    {
        [Test]
        public void ThrowAnException_WhenConstructedWithANullContext()
        {
            Assert.Throws<ArgumentNullException>(() => new SelectedSessionsLoader(null));
        }

        [Test]
        public void ThrowAnException_WhenGivenANullProfile()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionsLoader = new SelectedSessionsLoader(sessionRepository);
            Assert.Throws<ArgumentNullException>(() => sessionsLoader.LoadSessions(null));
        }

        [Test]
        public void OnlyReturnSessionsForTheSpecifiedSpeaker()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionsLoader = new SelectedSessionsLoader(sessionRepository);

            sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            sessionRepository.Received().GetSessionsSubmittedBy("bob");
        }

        [Test]
        public void OnlyReturnSelectedSessionsForTheSpecifiedSpeaker()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var session1 = new Session {SpeakerUserName = "bob", SessionId = 1234};
            var session3 = new Session {SpeakerUserName = "bob", SessionId = 44};
            sessionRepository.GetSessionsSubmittedBy("bob").Returns(new[] { session1, new Session { SpeakerUserName = "bob" }, session3 });
            var sessionsLoader = new SelectedSessionsLoader(sessionRepository);

            var sessions = sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            CollectionAssert.AreEqual(new[] {session3}, sessions);
        }
    }
}
