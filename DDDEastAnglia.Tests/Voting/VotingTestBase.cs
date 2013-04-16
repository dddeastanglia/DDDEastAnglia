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
        protected ICurrentUserVoteRepository CurrentUserVoteRepository;
        protected ISessionRepository SessionRepository;
        protected IEventRepository EventRepository;
        protected VotingCookie CookieWithNoVotes;
        protected VoteController Controller;
        protected IControllerInformationProvider ControllerInformationProvider;
        protected DateTime SimulatedNow;

        [SetUp]
        public void BeforeEachTest()
        {
            CurrentUserVoteRepository = Substitute.For<ICurrentUserVoteRepository>();
            SetCookieRepositoryExpectations(CurrentUserVoteRepository);

            EventRepository = Substitute.For<IEventRepository>();
            SetEventRepositoryExpectations(EventRepository);

            SessionRepository = Substitute.For<ISessionRepository>();
            SetSessionRepositoryExpectations(SessionRepository);

            ControllerInformationProvider = Substitute.For<IControllerInformationProvider>();
            SetRequestInformationProviderExpectations(ControllerInformationProvider);


            Controller = new VoteController(CurrentUserVoteRepository, SessionRepository, EventRepository, ControllerInformationProvider);
        }

        protected virtual void SetCookieRepositoryExpectations(ICurrentUserVoteRepository repository)
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

        protected virtual void SetRequestInformationProviderExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            SimulatedNow = DateTime.UtcNow;
            controllerInformationProvider.UtcNow.Returns(SimulatedNow);
        }
    }
}