using System.Collections.Generic;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISessionRepository
    {
        IEnumerable<Session> GetAllSessions();
        Session Get(int id);
        IEnumerable<Session> GetSessionsSubmittedBy(string speakerName);
        bool Exists(int id);
        Session AddSession(Session session);
        void UpdateSession(Session session);
        void DeleteSession(int id);
    }
}