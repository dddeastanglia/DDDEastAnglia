using DDDEastAnglia.DataAccess.SimpleData.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class ConferenceRepository : IConferenceRepository
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public Conference ForSession(int sessionId)
        {
            var session = db.Sessions.FindBySessionId(sessionId);
            return Get(session.ConferenceId);
        }

        public Conference GetByEventShortName(string shortName)
        {
            return db.Conferences.FindByShortName(shortName);
        }

        private Conference Get(int conferenceId)
        {
            return db.Conferences.FindByConferenceId(conferenceId);
        }
    }
}
