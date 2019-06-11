using System;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class SelectedSpeakerProfileFilterShould
    {
        [Test]
        public void ThrowAnException_WhenConstructedWithANullContext()
        {
            Assert.Throws<ArgumentNullException>(() => new SelectedSpeakerProfileFilter(null));
        }

        [Test]
        public void FilterOutUsersWhoHaveNotBeenSelected()
        {
            var profile1 = new UserProfile { UserId = 123 };
            var profile2 = new UserProfile { UserId = 48 };
            var profile3 = new UserProfile { UserId = 456 };

            var selectedSpeakerIds = new[] { profile1.UserId, profile3.UserId };

            var profileFilter = new SelectedSpeakerProfileFilter(selectedSpeakerIds);

            var profiles = profileFilter.FilterProfiles(new[] { profile1, profile2, profile3 });

            Assert.That(profiles, Is.EquivalentTo(new[] { profile1, profile3 }));
        }
    }
}
