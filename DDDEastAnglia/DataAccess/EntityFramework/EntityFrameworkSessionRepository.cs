using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkSessionRepository : ISessionRepository
    {

        public Session Get(int id)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                return dddeaContext.Sessions.Find(id);
            }
        }

        public bool Exists(int id)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                return dddeaContext.Sessions.Find(id) != null;
            }
        }
    }
}