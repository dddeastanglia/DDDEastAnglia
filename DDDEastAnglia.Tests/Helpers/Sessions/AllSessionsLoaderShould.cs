using System;
using DDDEastAnglia.DataAccess.EntityFramework;
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
            var db = Substitute.For<IDDDEAContext>();
            var sessionsLoader = new AllSessionsLoader(db);
            Assert.Throws<ArgumentNullException>(() => sessionsLoader.LoadSessions(null));
        }

        [Test]
        public void OnlyReturnSessionsForTheSpecifiedSpeaker()
        {
            var db = Substitute.For<IDDDEAContext>();
            var session1 = new Session {SpeakerUserName = "bob"};
            var session3 = new Session {SpeakerUserName = "bob"};
            db.Sessions.Returns(new[] {session1, new Session {SpeakerUserName = "fred"}, session3});
            var sessionsLoader = new AllSessionsLoader(db);

            var sessions = sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            CollectionAssert.AreEqual(new[] {session1, session3}, sessions);
        }
    }
}
