using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SelectedSpeakerProfileFilter : IUserProfileFilter
    {
        private readonly IEnumerable<int> selectedSpeakerIds;

        public SelectedSpeakerProfileFilter(IEnumerable<int> selectedSpeakerIds)
        {
            this.selectedSpeakerIds = selectedSpeakerIds ?? throw new ArgumentNullException(nameof(selectedSpeakerIds));
        }

        public IEnumerable<UserProfile> FilterProfiles(IEnumerable<UserProfile> profiles)
        {
            if (profiles == null)
            {
                throw new ArgumentNullException(nameof(profiles));
            }
            
            return profiles.Where(p => selectedSpeakerIds.Contains(p.UserId));
        }
    }
}
