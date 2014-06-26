using System;
using System.Web;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Am_Registering_A_Vote_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            var cookie = new HttpCookie(VotingCookie.CookieName, Guid.NewGuid().ToString());
            controllerInformationProvider.GetCookie(Arg.Any<string>()).Returns(cookie);
        }

        [Test]
        public void Record_The_SessionId()
        {
            Controller.RegisterVote(KnownSessionId);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.SessionId == KnownSessionId));
        }

        [Test]
        public void Record_The_Time_Of_The_Vote()
        {
            Controller.RegisterVote(1);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.TimeRecorded == SimulatedNow));
        }
    }
}
