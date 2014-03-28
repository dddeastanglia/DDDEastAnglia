using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IUserProfileRepository
    {
        IEnumerable<UserProfile> GetAllUserProfiles();
        UserProfile GetUserProfileById(int id);
        UserProfile GetUserProfileByUserName(string userName);
        UserProfile GetUserProfileByEmailAddress(string emailAddress);
        UserProfile AddUserProfile(UserProfile userProfile);
        void UpdateUserProfile(UserProfile profile);
    }
}
