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
    public class Given_That_I_Am_Removing_A_Vote_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;
        private readonly HttpCookie _httpCookie = new HttpCookie(VotingCookie.CookieName, CookieId.ToString());
        private static readonly Guid CookieId = Guid.NewGuid();

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(_httpCookie);
        }

        [Test]
        public void Send_A_DeleteVote_Command_With_The_Session_Id()
        {
            Controller.RemoveVote(KnownSessionId);

            MessageBus.Received().Send(Arg.Is<DeleteVoteCommand>(command => command.SessionId == KnownSessionId));
        }

        [Test]
        public void Send_A_DeleteVote_Command_With_The_Cookie_Id()
        {
            Controller.RemoveVote(KnownSessionId);

            MessageBus.Received().Send(Arg.Is<DeleteVoteCommand>(command => command.CookieId == CookieId));
        }
    }
}