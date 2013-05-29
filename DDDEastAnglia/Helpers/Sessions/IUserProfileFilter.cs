using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface IUserProfileFilter
    {
        IEnumerable<UserProfile> FilterProfiles(IEnumerable<UserProfile> profiles);
    }
}
