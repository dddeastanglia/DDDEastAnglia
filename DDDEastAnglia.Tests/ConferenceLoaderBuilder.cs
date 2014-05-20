﻿using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using NSubstitute;

namespace DDDEastAnglia.Tests
{
    public class ConferenceLoaderBuilder
    {
        private readonly IConference conference;
        private readonly IConferenceLoader conferenceLoader;

        public ConferenceLoaderBuilder()
        {
            conference = Substitute.For<IConference>();
            conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
        }

        public ConferenceLoaderBuilder WithSessionSubmissionClosed()
        {
            conference.CanSubmit().Returns(false);
            return this;
        }

        public ConferenceLoaderBuilder WithSessionSubmissionOpen()
        {
            conference.CanSubmit().Returns(true);
            return this;
        }

        public ConferenceLoaderBuilder WithVotingOpen()
        {
            conference.CanVote().Returns(true);
            return this;
        }

        public ConferenceLoaderBuilder WithAgendaNotPublished()
        {
            conference.CanPublishAgenda().Returns(false);
            return this;
        }

        public ConferenceLoaderBuilder WithAgendaPublished()
        {
            conference.CanPublishAgenda().Returns(true);
            return this;
        }

        public ConferenceLoaderBuilder WithRegistrationNotOpen()
        {
            conference.CanRegister().Returns(false);
            return this;
        }

        public ConferenceLoaderBuilder WithRegistrationOpen()
        {
            conference.CanRegister().Returns(true);
            return this;
        }

        public IConferenceLoader Build()
        {
            return conferenceLoader;
        }
    }
}
