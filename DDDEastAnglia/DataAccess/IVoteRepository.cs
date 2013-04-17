using System;
using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.DataAccess
{
    public interface IVoteRepository
    {
        void Save(Vote vote);
        void Delete(int sessionId, Guid cookieId);
    }
}