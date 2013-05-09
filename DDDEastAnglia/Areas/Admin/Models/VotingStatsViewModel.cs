using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess.EntityFramework.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class VotingStatsViewModel
    {
        public int TotalVotes{get;set;}
        public int NumberOfUsersWhoHaveVoted{get;set;}
        public IList<IGrouping<DateTime, Vote>> VotesByDate{get;set;}
        public IList<IGrouping<int, Vote>> VotesByHour{get;set;}
        public int NumberOfDaysOfVotingPassed{get;set;}
        public int NumberOfDaysOfVotingRemaining{get;set;}
        public int NumberOfDaysOfVoting{get;set;}
        public int VotingCompletePercentage{get;set;}
        public DateTime VotingStartDate{get;set;}
        public DateTime VotingEndDate{get;set;}
    }
}