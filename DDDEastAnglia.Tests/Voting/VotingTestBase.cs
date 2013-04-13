using System;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    public class VotingTestBase
    {
        protected const string DefaultSessionID = "THIS IS A SESSION ID";
        protected IVotingCookieRepository CookieRepository;
        protected IVoteRepository VoteRepository;
        protected ISessionRepository SessionRepository;
        protected IEventRepository EventRepository;
        protected VotingCookie CookieWithNoVotes;
        protected VoteController Controller;
        protected ITimeProvider TimeProvider;
        protected IRequestInformationProvider RequestInformationProvider;
        protected DateTime SimulatedNow;

        [SetUp]
        public void BeforeEachTest()
        {
            CookieRepository = Substitute.For<IVotingCookieRepository>();
            SetCookieRepositoryExpectations(CookieRepository);

            VoteRepository = Substitute.For<IVoteRepository>();
            SetVoteRepositoryExpectations(VoteRepository);

            EventRepository = Substitute.For<IEventRepository>();
            SetEventRepositoryExpectations(EventRepository);

            SessionRepository = Substitute.For<ISessionRepository>();
            SetSessionRepositoryExpectations(SessionRepository);

            
            TimeProvider = Substitute.For<ITimeProvider>();
            SetTimeProviderExpectations(TimeProvider);

            RequestInformationProvider = Substitute.For<IRequestInformationProvider>();
            SetRequestInformationProviderExpectations(RequestInformationProvider);


            Controller = new VoteController(CookieRepository, VoteRepository, SessionRepository, EventRepository, TimeProvider, RequestInformationProvider);
        }

        protected virtual void SetCookieRepositoryExpectations(IVotingCookieRepository repository)
        {
        }

        protected virtual void SetVoteRepositoryExpectations(IVoteRepository voteRepository)
        {
            
        }

        protected virtual void SetEventRepositoryExpectations(IEventRepository eventRepository)
        {
            eventRepository.Get(Arg.Is("DDDEA2013")).Returns(EventHelper.BuildEvent(true, true));
        }

        protected virtual void SetSessionRepositoryExpectations(ISessionRepository sessionRepository)
        {
        
        }

        protected virtual void SetTimeProviderExpectations(ITimeProvider timeProvider)
        {
            SimulatedNow = DateTime.UtcNow;
            timeProvider.UtcNow.Returns(SimulatedNow);
        }

        protected virtual void SetRequestInformationProviderExpectations(IRequestInformationProvider requestInformationProvider)
        {
            requestInformationProvider.SessionId.Returns(DefaultSessionID);
        }
    }
}