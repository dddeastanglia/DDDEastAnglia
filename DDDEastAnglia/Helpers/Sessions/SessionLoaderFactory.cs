using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface ISessionLoaderFactory
    {
        ISessionLoader Create(IConference conference);
    }

    public sealed class SessionLoaderFactory : ISessionLoaderFactory
    {
        private readonly ISessionRepository sessionRepository;

        public SessionLoaderFactory(ISessionRepository sessionRepository)
        {
            this.sessionRepository = sessionRepository ?? throw new ArgumentNullException(nameof(sessionRepository));
        }

        public ISessionLoader Create(IConference conference)
        {
            ISessionLoader sessionLoader;

            if (conference.CanPublishAgenda())
            {
                sessionLoader = new SelectedSessionsLoader(sessionRepository, SelectedSessions.SessionIds);
            }
            else
            {
                sessionLoader = new AllSessionsLoader(sessionRepository);
            }

            return sessionLoader;
        }
    }
}
