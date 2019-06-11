using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface IUserProfileFilterFactory
    {
        IUserProfileFilter Create(IConference conference);
    }

    public sealed class UserProfileFilterFactory : IUserProfileFilterFactory
    {
        private readonly ISessionRepository sessionRepository;

        public UserProfileFilterFactory(ISessionRepository sessionRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            this.sessionRepository = sessionRepository;
        }

        public IUserProfileFilter Create(IConference conference)
        {
            IUserProfileFilter userProfileFilter;

            if (conference.CanPublishAgenda())
            {
                userProfileFilter = new SelectedSpeakerProfileFilter(SelectedSessions.SpeakerIds);
            }
            else
            {
                userProfileFilter = new SubmittedSessionProfileFilter(sessionRepository);
            }

            return userProfileFilter;
        }
    }
}
