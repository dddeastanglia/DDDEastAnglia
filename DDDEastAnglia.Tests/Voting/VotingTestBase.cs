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
        private ISessionVoteModelQuery _sessionVoteModelQuery;
        private IMessageBus _messageBus;
        protected DateTime SimulatedNow;

        [SetUp]
        public void BeforeEachTest()
        {
            _controllerInformationProvider = Substitute.For<IControllerInformationProvider>();
            SetExpectations(_controllerInformationProvider);

            _sessionVoteModelQuery = Substitute.For<ISessionVoteModelQuery>();
            SetExpectations(_sessionVoteModelQuery);

            _messageBus = Substitute.For<IMessageBus>();
            SetExpectations(_messageBus);

            Controller = new VoteController(_messageBus, _sessionVoteModelQuery, _controllerInformationProvider);
        }

        protected IMessageBus MessageBus { get { return _messageBus; } }

        protected virtual void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            SimulatedNow = DateTime.UtcNow;
            controllerInformationProvider.UtcNow.Returns(SimulatedNow);
        }

        protected virtual void SetExpectations(ISessionVoteModelQuery sessionVoteModelQuery)
        {
            
        }

        protected virtual void SetExpectations(IMessageBus messageBus)
        {
            
        }

    }
}