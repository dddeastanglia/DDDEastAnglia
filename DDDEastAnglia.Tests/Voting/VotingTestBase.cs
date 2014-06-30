using System;
using System.Web;
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
        protected const string CookieName = "COOKIE_NAME";
        protected readonly Guid CookieId = Guid.NewGuid();

        protected VoteController Controller;
        protected IMessageBus MessageBus;
        protected DateTime SimulatedNow;
        protected IControllerInformationProvider ControllerInformationProvider;
        
        private ISessionVoteModelQuery sessionVoteModelQuery;

        [SetUp]
        public void BeforeEachTest()
        {
            SimulatedNow = DateTime.UtcNow;

            ControllerInformationProvider = Substitute.For<IControllerInformationProvider>();
            ControllerInformationProvider.UtcNow.Returns(SimulatedNow);
            SetExpectations(ControllerInformationProvider);

            var cookie = new HttpCookie(CookieName, CookieId.ToString());
            ControllerInformationProvider.GetCookie().Returns(cookie);

            sessionVoteModelQuery = Substitute.For<ISessionVoteModelQuery>();
            MessageBus = Substitute.For<IMessageBus>();

            Controller = new VoteController(sessionVoteModelQuery, MessageBus, ControllerInformationProvider);
        }

        protected virtual void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        { }
    }
}
