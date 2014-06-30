using DDDEastAnglia.DataAccess.Commands.Vote;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Have_Voted_For_Several_Sessions_The_VoteController_Should : VotingTestBase
    {
        private const int SessionNotVotedFor = 3;

        [Test]
        public void Record_A_Vote_For_A_Session_That_I_Have_Not_Voted_For()
        {
            Controller.RegisterVote(SessionNotVotedFor);
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.SessionId == SessionNotVotedFor));
        }
    }
}
