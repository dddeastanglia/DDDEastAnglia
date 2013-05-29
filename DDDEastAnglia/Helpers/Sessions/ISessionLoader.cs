using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Sessions
{
    public interface ISessionLoader
    {
        IEnumerable<Session> LoadSessions(UserProfile profile);
    }
}
