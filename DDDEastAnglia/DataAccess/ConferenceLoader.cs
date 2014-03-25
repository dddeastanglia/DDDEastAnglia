using System;
using DDDEastAnglia.DataAccess.SimpleData.Builders;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.DataAccess
{
    public interface IConferenceLoader
    {
        IConference LoadConference();
        IConference LoadConference(int sessionId);
    }

    public class ConferenceLoader : IConferenceLoader
    {
        private const string DefaultEventName = "DDDEA2013";

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
            return conferenceBuilder.Build(dataConference);
        }

        public IConference LoadConference(int sessionId)
        {
            var dataConference = conferenceRepository.ForSession(sessionId);
            return conferenceBuilder.Build(dataConference);
        }
    }
}
