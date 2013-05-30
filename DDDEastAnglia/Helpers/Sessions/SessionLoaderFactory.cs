using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface ISessionLoaderFactory
    {
        ISessionLoader Create(IConference conference, IDDDEAContext context);
    }

    public class SessionLoaderFactory : ISessionLoaderFactory
    {
        public ISessionLoader Create(IConference conference, IDDDEAContext context)
        {
            ISessionLoader sessionLoader;

            if (conference.CanPublishAgenda())
            {
                sessionLoader = new SelectedSessionsLoader(context);
            }
            else
            {
                sessionLoader = new AllSessionsLoader(context);
            }

            return sessionLoader;
        }
    }
}
