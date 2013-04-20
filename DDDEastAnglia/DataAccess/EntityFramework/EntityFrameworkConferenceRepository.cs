using System.Linq;
using DDDEastAnglia.DataAccess.Builders;
using Conference = DDDEastAnglia.Domain.Conference;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkConferenceRepository : IConferenceRepository
    {
        private readonly IBuild<Models.Conference, Conference> _conferenceBuilder;
        private readonly DDDEAContext _context = new DDDEAContext();

        public EntityFrameworkConferenceRepository(IBuild<Models.Conference, Conference> conferenceBuilder)
        {
            _conferenceBuilder = conferenceBuilder;
        }

        public Conference ForSession(int sessionId)
        {
            var session = _context.Sessions.Find(sessionId);
            return Get(session.ConferenceId);
        }

        private Conference Get(int conferenceId)
        {
            var currentConference = _context.Conferences
                                            .Include("CalendarItems")
                                            .SingleOrDefault(conf => conf.ConferenceId == conferenceId);
            return _conferenceBuilder.Build(currentConference);
        }
    }
}