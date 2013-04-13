using System;
using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.DataModel
{
    public class Vote
    {
        public int VoteId { get; set; }
        [Required]
        public string Event { get; set; }
        [Required]
        public int SessionId { get; set; }
        [Required]
        public Guid CookieId { get; set; }
        [Required]
        public bool IsVote { get; set; }
        [Required]
        public DateTime TimeRecorded { get; set; }
        public int UserId { get; set; } // just in case they are logged in
        public string IPAddress { get; set; }
        public long WebSessionId { get; set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }
        public string ScreenResolution { get; set; }
    }
}