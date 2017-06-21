using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface ISessionLoaderFactory
    {
        ISessionLoader Create(IConference conference);
    }

    public class SessionLoaderFactory : ISessionLoaderFactory
    {
        private readonly ISessionRepository sessionRepository;

        public SessionLoaderFactory(ISessionRepository sessionRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            this.sessionRepository = sessionRepository;
        }

        public ISessionLoader Create(IConference conference)
        {
            ISessionLoader sessionLoader;

            if (conference.CanPublishAgenda())
            {
                sessionLoader = new SelectedSessionsLoader(sessionRepository);
            }
            else
            {
                sessionLoader = new AllSessionsLoader(sessionRepository);
            }

            return sessionLoader;
        }
    }
}
