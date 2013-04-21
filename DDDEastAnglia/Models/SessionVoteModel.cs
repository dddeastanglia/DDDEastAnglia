namespace DDDEastAnglia.Models
{
    public class SessionVoteModel
    {
        public int SessionId { get; set; }
        public bool CanVote { get; set; }
        public bool HasBeenVotedForByUser { get; set; }
    }
}