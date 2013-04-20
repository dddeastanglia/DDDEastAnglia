using System;
using DDDEastAnglia.DataAccess.EntityFramework.Models;

namespace DDDEastAnglia.Helpers
{
    public interface ICurrentUserVoteRepository
    {
        bool HasVotedFor(int sessionId);
        void Save(Vote vote);
        void Delete(int sessionId);
    }
}