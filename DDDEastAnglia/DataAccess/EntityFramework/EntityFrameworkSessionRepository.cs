using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkSessionRepository : ISessionRepository
    {
        private readonly DDDEAContext context = new DDDEAContext();

        public Session Get(int id)
        {
            return context.Sessions.Find(id);
        }

        public bool Exists(int id)
        {
            return context.Sessions.Find(id) != null;
        }
    }
}