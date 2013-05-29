using System;
using DDDEastAnglia.DataAccess.EntityFramework;
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
            var db = Substitute.For<IDDDEAContext>();
            var sessionsLoader = new SelectedSessionsLoader(db);
            Assert.Throws<ArgumentNullException>(() => sessionsLoader.LoadSessions(null));
        }

        [Test]
        public void OnlyReturnSelectedSessionsForTheSpecifiedSpeaker()
        {
            var db = Substitute.For<IDDDEAContext>();
            var session1 = new Session {SpeakerUserName = "bob", SessionId = 1234};
            var session3 = new Session {SpeakerUserName = "bob", SessionId = 44};
            db.Sessions.Returns(new[] {session1, new Session {SpeakerUserName = "fred"}, session3});
            var sessionsLoader = new SelectedSessionsLoader(db);

            var sessions = sessionsLoader.LoadSessions(new UserProfile {UserName = "bob"});

            CollectionAssert.AreEqual(new[] {session3}, sessions);
        }
    }
}
