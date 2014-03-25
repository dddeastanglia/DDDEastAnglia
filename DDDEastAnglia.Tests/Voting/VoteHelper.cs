using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Tests.Voting
{
    public static class VoteHelper
    {
        public static bool IsVoteFor(this Vote vote, int expectedSessionId)
        {
            return vote.SessionId == expectedSessionId;
        }

        public static bool VoteHasBeenRemoved(this Vote vote, int sessionRemoved)
        {
            return vote.SessionId == sessionRemoved;
        }
    }
}