using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SubmittedSessionProfileFilter : IUserProfileFilter
    {
        private readonly ISessionRepository sessionRepository;

        public SubmittedSessionProfileFilter(ISessionRepository sessionRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            this.sessionRepository = sessionRepository;
        }

        public IEnumerable<UserProfile> FilterProfiles(IEnumerable<UserProfile> profiles)
        {
            return profiles.Where(profile =>
            {
                var submittedSessions = sessionRepository.GetSessionsSubmittedBy(profile.UserName);
                return submittedSessions != null && submittedSessions.Any(s => s.SpeakerUserName == profile.UserName);
            });
        }
    }
}
