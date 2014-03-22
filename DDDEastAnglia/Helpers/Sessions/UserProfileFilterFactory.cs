using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface IUserProfileFilterFactory
    {
        IUserProfileFilter Create(IConference conference);
    }

    public class UserProfileFilterFactory : IUserProfileFilterFactory
    {
        private readonly ISessionRepository sessionRepository;

        public UserProfileFilterFactory(ISessionRepository sessionRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException("sessionRepository");
            }
            
            this.sessionRepository = sessionRepository;
        }

        public IUserProfileFilter Create(IConference conference)
        {
            IUserProfileFilter userProfileFilter;

            if (conference.CanPublishAgenda())
            {
                userProfileFilter = new SelectedSpeakerProfileFilter();
            }
            else
            {
                userProfileFilter = new SubmittedSessionProfileFilter(sessionRepository);
            }

            return userProfileFilter;
        }
    }
}