using System;
using System.Linq;
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

        public void Delete(int sessionId, Guid cookieId)
        {
            var toDelete = context.Vote.FirstOrDefault(vote => vote.SessionId == sessionId && vote.CookieId == cookieId);
            if (toDelete == null)
            {
                return;
            }
            context.Vote.Remove(toDelete);
            context.SaveChanges();
        }
    }
}