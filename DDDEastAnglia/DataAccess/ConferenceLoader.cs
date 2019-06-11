using System;
using DDDEastAnglia.DataAccess.SimpleData.Builders;
using DDDEastAnglia.Domain;
using Conference = DDDEastAnglia.DataAccess.SimpleData.Models.Conference;

namespace DDDEastAnglia.DataAccess
{
    public interface IConferenceLoader
    {
        IConference LoadConference();
        IConference LoadConference(int sessionId);
    }

    public class ConferenceLoader : IConferenceLoader
    {
        private const string DefaultEventName = "DDDEA";

        private readonly IConferenceRepository conferenceRepository;
        private readonly ConferenceBuilder conferenceBuilder;

        public ConferenceLoader(IConferenceRepository conferenceRepository, ConferenceBuilder conferenceBuilder)
        {
            if (conferenceRepository == null)
            {
                throw new ArgumentNullException("conferenceRepository");
            }

            if (conferenceBuilder == null)
            {
                throw new ArgumentNullException("conferenceBuilder");
            }

            this.conferenceRepository = conferenceRepository;
            this.conferenceBuilder = conferenceBuilder;
        }

        public IConference LoadConference()
        {
            var dataConference = conferenceRepository.GetByEventShortName(DefaultEventName);
            return BuildConference(dataConference);
        }

        public IConference LoadConference(int sessionId)
        {
            var dataConference = conferenceRepository.ForSession(sessionId);
            return BuildConference(dataConference);
        }

        private IConference BuildConference(Conference conference)
        {
            if (conference == null)
            {
                throw new ArgumentNullException(nameof(conference), "Cannot find current conference");
            }

            return conferenceBuilder.Build(conference);
        }
    }
}
