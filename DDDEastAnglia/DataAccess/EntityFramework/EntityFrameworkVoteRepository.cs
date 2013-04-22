using System;
using System.Linq;
using DDDEastAnglia.DataAccess.EntityFramework.Models;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkVoteRepository : IVoteRepository
    {
        public void Save(Vote vote)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                dddeaContext.Votes.Add(vote);
                dddeaContext.SaveChanges();
            }
        }

        public void Delete(int sessionId, Guid cookieId)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                var toDelete =
                    dddeaContext.Votes.FirstOrDefault(vote => vote.SessionId == sessionId && vote.CookieId == cookieId);
                if (toDelete == null)
                {
                    return;
                }
                dddeaContext.Votes.Remove(toDelete);
                dddeaContext.SaveChanges();
            }
        }

        public bool HasVotedFor(int sessionId, Guid cookieId)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                return dddeaContext.Votes.Any(vote => vote.CookieId == cookieId && vote.SessionId == sessionId);
            }
        }
    }
}