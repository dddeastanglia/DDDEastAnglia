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
            var sessionsLoader = new AllSessionsLoader(sessionRepository);

            sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            sessionRepository.Received().GetSessionsSubmittedBy("bob");
        }
    }
}
