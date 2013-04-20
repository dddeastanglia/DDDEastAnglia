﻿using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataAccess.Handlers.Voting;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Context;

namespace DDDEastAnglia.DataAccess
{
    public static class Factory
    {
        private static readonly object Mutex = new object();
        private static IConferenceRepository _conferenceRepository;
        private static IVoteRepository _voteRepository;
        private static SimpleMessageBus _simpleMessageBus;

        public static ISessionVoteModelProvider GetSessionVoteModelProvider()
        {
            return new SessionVoteModelProvider(GetVoteRepository(), GetConferenceRepository());
        }

        public static IMessageBus GetMessageBus()
        {
            if (_simpleMessageBus == null)
            {
                lock (Mutex)
                {
                    _simpleMessageBus = _simpleMessageBus ?? CreateNewSimpleMessageBus();
                }
            }
            return _simpleMessageBus;
        }

        public static IControllerInformationProvider GetControllerInformationProvider()
        {
            return new HttpContextControllerInformationProvider();
        }

        public static IVoteRepository GetVoteRepository()
        {
            if (_voteRepository == null)
            {
                lock (Mutex)
                {
                    _voteRepository = _voteRepository ?? new EntityFrameworkVoteRepository();
                }
            }
            return _voteRepository;   
        }

        public static IConferenceRepository GetConferenceRepository()
        {
            if (_conferenceRepository == null)
            {
                lock (Mutex)
                {
                    var builder = new ConferenceBuilder(
                                        new CalendarEntryBuilder(new CalendarItemToSingleTimeEntryConverter(),
                                                                 new CalendarItemToTimeRangeEntryConverter()));
                    _conferenceRepository = _conferenceRepository ?? new EntityFrameworkConferenceRepository(builder);
                }
            }
            return _conferenceRepository;
        }

        private static SimpleMessageBus CreateNewSimpleMessageBus()
        {
            var simpleMessageBus = new SimpleMessageBus();
            simpleMessageBus.Register(new DeleteVoteCommandHandler(GetVoteRepository(), GetConferenceRepository()));
            simpleMessageBus.Register(new RegisterVoteCommandHandler(GetVoteRepository(), GetConferenceRepository()));
            return simpleMessageBus;
        }
    }
}