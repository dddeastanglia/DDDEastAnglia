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
    public class Given_The_Session_Position_Is_Specified_When_Posting_The_Data_The_VoteController_Should : VotingTestBase
    {
        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            base.SetExpectations(controllerInformationProvider);
            var cookieId = Guid.NewGuid();
            var httpCookie = new HttpCookie(VotingCookie.CookieName, cookieId.ToString());
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(httpCookie);
        }

        [Test]
        public void Save_The_Position_With_The_Vote()
        {
            const int sessionIdToVoteFor = 1;
            Controller.RegisterVote(sessionIdToVoteFor, new VoteModel { PositionInList = 5 });
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.PositionInList == 5));
        }
    }
}
