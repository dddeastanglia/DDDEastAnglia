

using DDDEastAnglia.Domain;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkConferenceRepository : IConferenceRepository
    {
        private readonly DDDEAContext _context = new DDDEAContext();

        public Conference ForSession(int sessionId)
        {
            var session = _context.Sessions.Find(sessionId);
            var currentConference = _context.Conferences.Find(session.ConferenceId);
            return new Conference(currentConference.ConferenceId, currentConference.Name, currentConference.ShortName);

        }
    }
}