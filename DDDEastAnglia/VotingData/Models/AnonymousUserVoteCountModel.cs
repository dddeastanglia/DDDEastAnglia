using System;

namespace DDDEastAnglia.VotingData.Models
{
    public class AnonymousUserVoteCountModel
    {
        public Guid CookieId{get;set;} 
        public int NumberOfVotes{get;set;} 
    }
}