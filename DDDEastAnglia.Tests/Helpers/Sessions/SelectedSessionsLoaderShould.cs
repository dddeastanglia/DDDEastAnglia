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
            Assert.Throws<ArgumentNullException>(() => new SelectedSessionsLoader(null, new int[0]));
        }

        [Test]
        public void ThrowAnException_WhenConstructedWithANullListOfSessions()
        {
            Assert.Throws<ArgumentNullException>(() => new SelectedSessionsLoader(Substitute.For<ISessionRepository>(), null));
        }

        [Test]
        public void ThrowAnException_WhenGivenANullProfile()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionsLoader = new SelectedSessionsLoader(sessionRepository, new int[0]);
            Assert.Throws<ArgumentNullException>(() => sessionsLoader.LoadSessions(null));
        }

        [Test]
        public void OnlyReturnSessionsForTheSpecifiedSpeaker()
        {
            var sessionRepository = Substitute.For<ISessionRepository>();
            var sessionsLoader = new SelectedSessionsLoader(sessionRepository, new int[0]);

            sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            sessionRepository.Received().GetSessionsSubmittedBy("bob");
        }

        [Test]
        public void OnlyReturnSelectedSessionsForTheSpecifiedSpeaker()
        {
            var session1 = new Session { SpeakerUserName = "bob", SessionId = 1 };
            var session2 = new Session { SpeakerUserName = "fred", SessionId = 2 };
            var session3 = new Session { SpeakerUserName = "bob", SessionId = 3 };

            var sessionRepository = Substitute.For<ISessionRepository>();
            sessionRepository.GetSessionsSubmittedBy("bob").Returns(new[] { session1, session3 });

            var selectedSessionIds = new[] { session1.SessionId, session2.SessionId, session3.SessionId };
            var sessionsLoader = new SelectedSessionsLoader(sessionRepository, selectedSessionIds);

            var sessions = sessionsLoader.LoadSessions(new UserProfile { UserName = "bob" });

            Assert.That(sessions, Is.EquivalentTo(new[] { session1, session3 }));
        }
    }
}
