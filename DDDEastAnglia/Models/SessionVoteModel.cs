namespace DDDEastAnglia.Models
{
    public class SessionVoteModel
    {
        public int SessionId { get; set; }
        public bool UserCanVote { get; set; }
        public bool VotedForByUser { get; set; }
    }
}