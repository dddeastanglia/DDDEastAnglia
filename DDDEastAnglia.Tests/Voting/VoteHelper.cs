using DDDEastAnglia.DataModel;

namespace DDDEastAnglia.Tests.Voting
{
    public static class VoteHelper
    {
        public static bool IsVoteFor(this Vote vote, string eventId, int expectedSessionId)
        {
            if (!vote.Event.Equals(eventId))
            {
                return false;
            }
            if (vote.SessionId != expectedSessionId)
            {
                return false;
            }
            if (!vote.IsVote)
            {
                return false;
            }
            return true;
        }

        public static bool VoteHasBeenRemoved(this Vote vote, string eventId, int sessionRemoved)
        {
            if (!vote.Event.Equals(eventId))
            {
                return false;
            }
            if (vote.SessionId != sessionRemoved)
            {
                return false;
            }
            if (vote.IsVote)
            {
                return false;
            }
            return true;
        }

    }
}