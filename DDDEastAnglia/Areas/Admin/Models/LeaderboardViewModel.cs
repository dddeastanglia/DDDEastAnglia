using System.Collections.Generic;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class LeaderboardViewModel
    {
        public int HighestVoteCount{get;set;}
        public IList<LeaderBoardSession> Sessions{get;set;}
    }
}