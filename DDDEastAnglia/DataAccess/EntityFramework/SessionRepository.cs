using System.Collections.Generic;
using DDDEastAnglia.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class SessionRepository : ISessionRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<Session> GetAllSessions()
        {
            return db.Sessions.All();
        }

        public Session Get(int id)
        {
            return db.Sessions.FindBySessionId(id);
        }

        public IEnumerable<Session> GetSessionsSubmittedBy(string speakerName)
        {
            return db.Sessions.FindAllBySpeakerUserName(speakerName);
        }

        public bool Exists(int id)
        {
            return db.Sessions.Exists(db.Sessions.id == id);
        }

        public Session AddSession(Session session)
        {
            return db.Sessions.Insert(session);
        }

        public void UpdateSession(Session session)
        {
            db.Sessions.UpdateBySessionId(session);
        }

        public void DeleteSession(int id)
        {
            db.Sessions.DeleteBySessionId(id);
        }
    }
}