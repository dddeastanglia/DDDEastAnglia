namespace DDDEastAnglia.Models
{
    public class SessionDisplayModel
    {
        public string SessionAbstract { get; set; }
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerUserName { get; set; }
        public string SpeakerGravitarUrl { get; set; }
        public string TweetLink { get; set; }
        public bool IsUsersSession { get; set; }
    }
}