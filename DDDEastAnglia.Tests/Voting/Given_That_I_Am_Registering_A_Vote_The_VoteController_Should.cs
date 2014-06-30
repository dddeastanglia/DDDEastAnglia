using DDDEastAnglia.DataAccess.Commands.Vote;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_That_I_Am_Registering_A_Vote_The_VoteController_Should : VotingTestBase
    {
        private const int KnownSessionId = 1;

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
