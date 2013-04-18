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
        protected VoteController Controller;
        private IControllerInformationProvider _controllerInformationProvider;
        private IBuild<SessionVoteModel> _sessionVoteModelBuilder;
        private IMessageBus _messageBus;
        protected DateTime SimulatedNow;

        [SetUp]
        public void BeforeEachTest()
        {
            _controllerInformationProvider = Substitute.For<IControllerInformationProvider>();
            SetExpectations(_controllerInformationProvider);

            _sessionVoteModelBuilder = Substitute.For<IBuild<SessionVoteModel>>();
            SetExpectations(_sessionVoteModelBuilder);

            _messageBus = Substitute.For<IMessageBus>();
            SetExpectations(_messageBus);

            Controller = new VoteController(_sessionVoteModelBuilder, _messageBus, _controllerInformationProvider);
        }

        protected IMessageBus MessageBus { get { return _messageBus; } }

        protected virtual void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            SimulatedNow = DateTime.UtcNow;
            controllerInformationProvider.UtcNow.Returns(SimulatedNow);
        }

        protected virtual void SetExpectations(IBuild<SessionVoteModel> sessionVoteModelBuilder)
        {
            
        }

        protected virtual void SetExpectations(IMessageBus messageBus)
        {
            
        }

    }
}