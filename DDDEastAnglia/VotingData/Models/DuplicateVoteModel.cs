using System;

namespace DDDEastAnglia.VotingData.Models
{
    public class DuplicateVoteModel
    {
        public Guid CookieId{get;set;}
        public int? UserId{get;set;}
        public string GravatarUrl{get;set;}
        public int SessionId{get;set;}
        public string SessionTitle{get;set;}
        public int SpeakerUserId{get;set;}
        public string SpeakerName{get;set;}
        public int NumberOfVotes{get;set;}
    }
}
