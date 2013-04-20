using System;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.Helpers
{
    public interface ICurrentUserVoteRepository
    {
        bool HasVotedFor(int sessionId);
        void Save(Vote vote);
        void Delete(int sessionId);
    }
}