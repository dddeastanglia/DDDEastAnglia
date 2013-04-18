using System;

namespace DDDEastAnglia.DataAccess.Commands.Vote
{
    public class RegisterVoteCommand : ICommand
    {
        public int SessionId { get; set; }
        public DateTime TimeRecorded { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string Referrer { get; set; }
        public string WebSessionId { get; set; }
        public int UserId { get; set; }
        public string ScreenResolution { get; set; }
        public Guid CookieId { get; set; }
    }
}