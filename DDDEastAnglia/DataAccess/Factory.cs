using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataAccess.Handlers.Voting;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Context;
using DDDEastAnglia.Models;
using DDDEastAnglia.Models.Builders;

namespace DDDEastAnglia.DataAccess
{
    public static class Factory
    {
        private static readonly object Mutex = new object();
        private static IConferenceRepository _conferenceRepository;
        private static IVoteRepository _voteRepository;
        private static SimpleMessageBus _simpleMessageBus;

        public static IBuild<SessionVoteModel> GetSessionVoteModelBuilder()
        {
            return new SessionVoteModelBuilder(GetControllerInformationProvider(), GetVoteRepository(), GetConferenceRepository());
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
                    _conferenceRepository = _conferenceRepository ?? new EntityFrameworkConferenceRepository();
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