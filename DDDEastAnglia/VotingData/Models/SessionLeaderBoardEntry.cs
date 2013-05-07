namespace DDDEastAnglia.VotingData.Models
{
    public class SessionLeaderBoardEntry
    {
        public int Position{get;set;}
        public int SessionId{get;set;}
        public string SessionTitle{get;set;}
        public int SpeakerUserId{get;set;}
        public string SpeakerName{get;set;}
        public int NumberOfVotes{get;set;}
    }
}