using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkVoteRepository : IVoteRepository
    {
        private DDDEAContext context = new DDDEAContext();

        public void Save(Vote vote)
        {
            context.Vote.Add(vote);
            context.SaveChanges();
        }
    }
}