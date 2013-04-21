using System;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    public class VotingTestBase
    {
        protected VoteController Controller;
        private IControllerInformationProvider _controllerInformationProvider;
        private ISessionVoteModelProvider _sessionVoteModelProvider;
        private IMessageBus _messageBus;
        protected DateTime SimulatedNow;

        [SetUp]
        public void BeforeEachTest()
        {
            _controllerInformationProvider = Substitute.For<IControllerInformationProvider>();
            SetExpectations(_controllerInformationProvider);

            _sessionVoteModelProvider = Substitute.For<ISessionVoteModelProvider>();
            SetExpectations(_sessionVoteModelProvider);

            _messageBus = Substitute.For<IMessageBus>();
            SetExpectations(_messageBus);

            Controller = new VoteController(_sessionVoteModelProvider, _messageBus, _controllerInformationProvider);
        }

        protected IMessageBus MessageBus { get { return _messageBus; } }

        protected virtual void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            SimulatedNow = DateTime.UtcNow;
            controllerInformationProvider.UtcNow.Returns(SimulatedNow);
        }

        protected virtual void SetExpectations(ISessionVoteModelProvider sessionVoteModelProvider)
        {
            
        }

        protected virtual void SetExpectations(IMessageBus messageBus)
        {
            
        }

    }
}