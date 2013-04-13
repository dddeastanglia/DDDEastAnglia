using System.Collections.Generic;

namespace DDDEastAnglia.Models
{
    public class SessionDisplayModel
    {
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }
        public string SessionAbstract { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerUserName { get; set; }
        public string SpeakerGravitarUrl { get; set; }
        public SessionTweetLink TweetLink { get; set; }
        public bool IsUsersSession { get; set; }
        public bool HasAlreadyBeenVotedFor { get; set; }
    }

    public class SessionIndexModel
    {
        public bool IsOpenForSubmission { get; set; }
        public bool IsOpenForVoting { get; set; }

        public IEnumerable<SessionDisplayModel> Sessions { get; set; }
    }
}
