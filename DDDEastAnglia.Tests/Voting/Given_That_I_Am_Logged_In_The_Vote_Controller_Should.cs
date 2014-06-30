using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Am_Logged_In_The_Vote_Controller_Should : VotingTestBase
    {
        private const int SessionIdToVoteFor = 1;
        private readonly UserProfile userProfile = new UserProfile { UserId = 100 };

        protected override void SetExpectations(IControllerInformationProvider controllerInformationProvider)
        {
            controllerInformationProvider.IsLoggedIn().Returns(true);
            controllerInformationProvider.GetCurrentUser().Returns(userProfile);
        }

        [Test]
        public void Save_My_UserId_With_The_Vote()
        {
            Controller.RegisterVote(SessionIdToVoteFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.UserId == userProfile.UserId));
        }
    }
}
