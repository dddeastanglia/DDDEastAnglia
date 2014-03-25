using System;
using System.Collections.Generic;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class VoteRepository : IVoteRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<Vote> GetAllVotes()
        {
            return db.Votes.All();
        }

        public void AddVote(Vote vote)
        {
            db.Votes.Insert(vote);
        }

        public void Delete(int sessionId, Guid cookieId)
        {
            db.Votes.Delete(SessionId : sessionId, cookieId : cookieId);
        }

        public bool HasVotedFor(int sessionId, Guid cookieId)
        {
            return db.Votes.Exists(db.Votes.CookieId == cookieId && db.Votes.SessionId == sessionId);
        }
    }
}
