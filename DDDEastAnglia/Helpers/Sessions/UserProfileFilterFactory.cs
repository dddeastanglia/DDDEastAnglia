using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface IUserProfileFilterFactory
    {
        IUserProfileFilter Create(IConference conference, IDDDEAContext context);
    }

    public class UserProfileFilterFactory : IUserProfileFilterFactory
    {
        public IUserProfileFilter Create(IConference conference, IDDDEAContext context)
        {
            IUserProfileFilter userProfileFilter;

            if (conference.CanPublishAgenda())
            {
                userProfileFilter = new SelectedSpeakerProfileFilter();
            }
            else
            {
                userProfileFilter = new SubmittedSessionProfileFilter(context);
            }

            return userProfileFilter;
        }
    }
}