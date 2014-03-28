using System;
using System.Collections.Generic;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IVoteRepository
    {
        IEnumerable<Vote> GetAllVotes();
        void AddVote(Vote vote);
        void Delete(int sessionId, Guid cookieId);
        bool HasVotedFor(int sessionId, Guid cookieId);
    }
}