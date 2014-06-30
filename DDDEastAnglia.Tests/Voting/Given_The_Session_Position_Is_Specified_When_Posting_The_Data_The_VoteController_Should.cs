using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Voting
{
    [TestFixture]
    public class Given_The_Session_Position_Is_Specified_When_Posting_The_Data_The_VoteController_Should : VotingTestBase
    {
        [Test]
        public void Save_The_Position_With_The_Vote()
        {
            const int sessionIdToVoteFor = 1;
            Controller.RegisterVote(sessionIdToVoteFor, new VoteModel { PositionInList = 5 });
            MessageBus.Received().Send(Arg.Is<RegisterVoteCommand>(command => command.PositionInList == 5));
        }
    }
}
