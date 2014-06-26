using System;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    public abstract class VotingTestBase
    {
        protected IVotingCookie VotingCookie;
        protected VoteController Controller;
        protected IMessageBus MessageBus;
        protected DateTime SimulatedNow;
        protected IControllerInformationProvider ControllerInformationProvider;
        
        private ISessionVoteModelQuery sessionVoteModelQuery;

        [SetUp]
        public void BeforeEachTest()
        {
            SimulatedNow = DateTime.UtcNow;

            VotingCookie = Substitute.For<IVotingCookie>();
            VotingCookie.CookieName.Returns("DDDEACookieName");
            VotingCookie.DefaultExpiry.Returns(SimulatedNow);

            ControllerInformationProvider = Substitute.For<IControllerInformationProvider>();
            ControllerInformationProvider.UtcNow.Returns(SimulatedNow);
            SetExpectations(ControllerInformationProvider);

            sessionVoteModelQuery = Substitute.For<ISessionVoteModelQuery>();
            MessageBus = Substitute.For<IMessageBus>();

            Controller = new VoteController(VotingCookie, sessionVoteModelQuery, MessageBus, ControllerInformationProvider);
        }

        protected abstract void SetExpectations(IControllerInformationProvider controllerInformationProvider);
    }
}
