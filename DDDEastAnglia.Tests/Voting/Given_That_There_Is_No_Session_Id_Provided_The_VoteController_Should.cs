using System;
using System.Web;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_There_Is_No_Session_Id_Provided_The_VoteController_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            var cookie = new HttpCookie(VotingCookie.CookieName, Guid.NewGuid().ToString());
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(cookie);
            controllerInformationProvider.SessionId.Returns((string)null);
        }

        [Test]
        public void Send_The_Command_With_A_Null_WebSessionId_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.WebSessionId == null));
        }
    }
}
