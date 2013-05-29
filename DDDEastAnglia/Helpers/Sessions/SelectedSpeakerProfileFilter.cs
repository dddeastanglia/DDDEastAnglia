using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SelectedSpeakerProfileFilter : IUserProfileFilter
    {
        public IEnumerable<UserProfile> FilterProfiles(IEnumerable<UserProfile> profiles)
        {
            return profiles.Where(p => SelectedSessions.SpeakerIds.Contains(p.UserId));
        }
    }
}
