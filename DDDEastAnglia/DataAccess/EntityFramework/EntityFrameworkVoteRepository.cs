using System;
using System.Linq;
using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkVoteRepository : IVoteRepository
    {
        private readonly DDDEAContext _context = new DDDEAContext();

        public void Save(Vote vote)
        {
            _context.Votes.Add(vote);
            _context.SaveChanges();
        }

        public void Delete(int sessionId, Guid cookieId)
        {
            var toDelete = _context.Votes.FirstOrDefault(vote => vote.SessionId == sessionId && vote.CookieId == cookieId);
            if (toDelete == null)
            {
                return;
            }
            _context.Votes.Remove(toDelete);
            _context.SaveChanges();
        }

        public bool HasVotedFor(int sessionId, Guid cookieId)
        {
            return _context.Votes.Any(vote => vote.CookieId == cookieId && vote.SessionId == sessionId);
        }
    }
}