using System;
using System.Web;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_A_SessionId_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        protected const string DefaultSessionID = "THIS IS A SESSION ID";
        private readonly HttpCookie _httpCookie = new HttpCookie(VotingCookie.CookieName, CookieId.ToString());
        private static readonly Guid CookieId = Guid.NewGuid();

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(_httpCookie);
            controllerInformationProvider.SessionId.Returns(DefaultSessionID);
        }

        [Test]
        public void Send_The_Command_With_The_Provided_WebSessionId()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.WebSessionId == DefaultSessionID));
        }
    }
}