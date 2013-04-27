namespace DDDEastAnglia.Models
{
    public class SessionDisplayModel
    {
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SessionAbstract { get; set; }
        
        public int SpeakerId { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerUserName { get; set; }
        public string SpeakerGravatarUrl { get; set; }
        
        public SessionTweetLink TweetLink { get; set; }
        public bool IsUsersSession { get; set; }
        public bool HasAlreadyBeenVotedFor { get; set; }
    }
}
