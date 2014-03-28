using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public class SelectedSessionsLoader : ISessionLoader
    {
        private readonly ISessionRepository sessionRepository;

        public SelectedSessionsLoader(ISessionRepository sessionRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException("sessionRepository");
            }
            
            this.sessionRepository = sessionRepository;
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException("profile");
            }

            var sessions = sessionRepository.GetSessionsSubmittedBy(profile.UserName);
            return sessions.Where(s => SelectedSessions.SessionIds.Contains(s.SessionId));
        }
    }
}
