using System;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISessionVoteModelQuery
    {
        SessionVoteModel Get(int sessionId, Guid cookieId);
    }
}