using System.Linq;
using DDDEastAnglia.DataAccess.Builders;
using Conference = DDDEastAnglia.Domain.Conference;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkConferenceRepository : IConferenceRepository
    {
        private readonly IBuild<Models.Conference, Conference> _conferenceBuilder;

        public EntityFrameworkConferenceRepository(IBuild<Models.Conference, Conference> conferenceBuilder)
        {
            _conferenceBuilder = conferenceBuilder;
        }

        public Conference ForSession(int sessionId)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                var session = dddeaContext.Sessions.Find(sessionId);
                return Get(session.ConferenceId);
            }
        }

        public Conference GetByEventShortName(string shortName)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                var currentConference = dddeaContext.Conferences
                                                    .Include("CalendarItems")
                                                    .SingleOrDefault(conf => conf.ShortName == shortName);
                return _conferenceBuilder.Build(currentConference);
            }
        }

        private Conference Get(int conferenceId)
        {
            using (var dddeaContext = new DDDEAContext())
            {
                var currentConference = dddeaContext.Conferences
                                                .Include("CalendarItems")
                                                .SingleOrDefault(conf => conf.ConferenceId == conferenceId);
                return _conferenceBuilder.Build(currentConference);
            }
        }
    }
}