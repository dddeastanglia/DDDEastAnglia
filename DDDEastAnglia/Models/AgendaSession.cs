namespace DDDEastAnglia.Models
{
    public sealed class AgendaSession
    {
        public int SessionId { get; set; }
        public string SessionTitle { get; set; }

        public int SpeakerUserId { get; set; }
        public string SpeakerName { get; set; }
    }
}
