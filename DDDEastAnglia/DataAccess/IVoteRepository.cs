using System;
using DDDEastAnglia.DataAccess.EntityFramework.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IVoteRepository
    {
        void Save(Vote vote);
        void Delete(int sessionId, Guid cookieId);
        bool HasVotedFor(int sessionId, Guid cookieId);
    }
}