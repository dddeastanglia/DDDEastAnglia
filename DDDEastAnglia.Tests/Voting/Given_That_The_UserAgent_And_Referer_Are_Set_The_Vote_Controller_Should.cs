using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_The_UserAgent_And_Referer_Are_Set_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private const string UserAgent = "A Browser";
        private const string Referrer = "http://www.referer.com";

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            controllerInformationProvider.UserAgent.Returns(UserAgent);
            controllerInformationProvider.Referrer.Returns(Referrer);
        }

        [Test]
        public void Save_The_UserAgent_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.UserAgent == UserAgent));
        }

        [Test]
        public void Save_The_Referer_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.Referrer == Referrer));
        }
    }
}
