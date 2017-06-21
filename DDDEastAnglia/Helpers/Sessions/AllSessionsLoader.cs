using System;
using System.Collections.Generic;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class AllSessionsLoader : ISessionLoader
    {
        private readonly ISessionRepository sessionRepository;

        public AllSessionsLoader(ISessionRepository sessionRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            this.sessionRepository = sessionRepository;
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            return sessionRepository.GetSessionsSubmittedBy(profile.UserName);
        }
    }
}
