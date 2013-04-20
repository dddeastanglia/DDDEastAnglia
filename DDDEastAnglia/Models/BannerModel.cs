namespace DDDEastAnglia.Models
{
    public class BannerModel
    {
        public bool IsOpenForSubmission { get; set; }
        public bool IsOpenForVoting { get; set; }
        public string SessionSubmissionCloses { get; set; }
        public string VotingCloses { get;  set; }
    }
}