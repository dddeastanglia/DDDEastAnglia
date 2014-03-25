using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class AllSessionsLoaderShould
    {
        [Test]
        public void ThrowAnException_WhenConstructedWithANullContext()
        {
            Assert.Throws<ArgumentNullException>(() => new AllSessionsLoader(null));
        }

        [Test]
        public void ThrowAnException_WhenGivenANullProfile()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionsLoader = new AllSessionsLoader(sessionRepository);
            Assert.Throws<ArgumentNullException>(() => sessionsLoader.LoadSessions(null));
        }

        [Test]
        public void OnlyReturnSessionsForTheSpecifiedSpeaker()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var session1 = new Session {SpeakerUserName = "bob"};
            var session3 = new Session {SpeakerUserName = "bob"};
            sessionRepository.GetAllSessions().Returns(new[] { session1, new Session { SpeakerUserName = "fred" }, session3 });
            var sessionsLoader = new AllSessionsLoader(sessionRepository);

            var sessions = sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            CollectionAssert.AreEqual(new[] {session1, session3}, sessions);
        }
    }
}
