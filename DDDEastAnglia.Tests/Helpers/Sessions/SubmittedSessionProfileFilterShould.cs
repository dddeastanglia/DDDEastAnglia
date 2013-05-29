using System;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class SubmittedSessionProfileFilterShould
    {
        [Test]
        public void ThrowAnException_WhenConstructedWithANullContext()
        {
            Assert.Throws<ArgumentNullException>(() => new SubmittedSessionProfileFilter(null));
        }

        [Test]
        public void FilterOutUsersWhoHaveNotSubmittedSessions()
        {
            var context = Substitute.For<IDDDEAContext>();
            var profileFilter = new SubmittedSessionProfileFilter(context);

            var profile1 = new UserProfile {UserName = "fred"};
            var profile2 = new UserProfile {UserName = "george"};
            var profile3 = new UserProfile {UserName = "bob"};

            context.Sessions.Returns(new[] {new Session {SpeakerUserName = "bob"}});

            var profiles = profileFilter.FilterProfiles(new[] {profile1, profile2, profile3});

            CollectionAssert.AreEquivalent(new[] {profile3}, profiles);
        }
    }
}
