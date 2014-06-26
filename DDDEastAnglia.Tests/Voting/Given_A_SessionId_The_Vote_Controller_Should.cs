using System;
using System.Web;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_A_SessionId_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const string DefaultSessionId = "THIS IS A SESSION ID";

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            var cookie = new HttpCookie(VotingCookie.CookieName, Guid.NewGuid().ToString());
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(cookie);
            controllerInformationProvider.SessionId.Returns(DefaultSessionId);
        }

        [Test]
        public void Send_The_Command_With_The_Provided_WebSessionId()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.WebSessionId == DefaultSessionId));
        }
    }
}
