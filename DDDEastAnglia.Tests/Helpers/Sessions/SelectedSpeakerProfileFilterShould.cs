using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers.Sessions
{
    [TestFixture]
    public sealed class SelectedSpeakerProfileFilterShould
    {
        [Test]
        public void FilterOutUsersWhoHaveNotBeenSelected()
        {
            var profileFilter = new SelectedSpeakerProfileFilter();

            var profile1 = new UserProfile {UserId = 123};
            var profile2 = new UserProfile {UserId = 48};
            var profile3 = new UserProfile {UserId = 456};

            var profiles = profileFilter.FilterProfiles(new[] {profile1, profile2, profile3});

            CollectionAssert.AreEquivalent(new[] {profile2}, profiles);
        }
    }
}
