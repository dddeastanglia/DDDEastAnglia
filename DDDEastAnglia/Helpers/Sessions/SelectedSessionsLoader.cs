using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public sealed class SelectedSessionsLoader : ISessionLoader
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IEnumerable<int> sessionIds;

        public SelectedSessionsLoader(ISessionRepository sessionRepository, IEnumerable<int> selectedSessionIds)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            if (selectedSessionIds == null)
            {
                throw new ArgumentNullException(nameof(selectedSessionIds));
            }

            this.sessionRepository = sessionRepository;
            this.sessionIds = selectedSessionIds;
        }

        public IEnumerable<Session> LoadSessions()
        {
            var sessions = sessionRepository.GetAllSessions().ToList();
            return Filter(sessions);
        }

        public IEnumerable<Session> LoadSessions(UserProfile profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            var sessions = sessionRepository.GetSessionsSubmittedBy(profile.UserName);
            return Filter(sessions);
        }

        private IEnumerable<Session> Filter(IEnumerable<Session> sessions)
        {
            return sessions.Where(s => sessionIds.Contains(s.SessionId)).ToList();
        }
    }
}
