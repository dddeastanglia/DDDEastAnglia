using System.Collections.Generic;
using DDDEastAnglia.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<UserProfile> GetAllUserProfiles()
        {
            return db.UserProfiles.All();
        }

        public UserProfile GetUserProfileById(int id)
        {
            return db.UserProfiles.FindByUserId(id);
        }

        public UserProfile GetUserProfileByUserName(string userName)
        {
            return db.UserProfiles.FindByUserName(userName);
        }

        public UserProfile GetUserProfileByEmailAddress(string emailAddress)
        {
            return db.UserProfiles.FindUserByEmailAddress(emailAddress);
        }

        public UserProfile AddUserProfile(UserProfile userProfile)
        {
            return db.UserProfiles.Insert(userProfile);
        }

        public void UpdateUserProfile(UserProfile profile)
        {
            db.UserProfiles.UpdateByUserId(profile);
        }
    }
}
