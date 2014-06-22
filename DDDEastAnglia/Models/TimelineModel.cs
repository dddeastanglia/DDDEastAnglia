namespace DDDEastAnglia.Models
{
    public class TimelineModel
    {
        public TimelineItemModel SessionSubmissionOpens { get; set; }
        public TimelineItemModel SessionSubmissionCloses { get; set; }
        public TimelineItemModel VotingOpens { get; set; }
        public TimelineItemModel VotingCloses { get; set; }
        public TimelineItemModel AgendaAnnounced { get; set; }
        public TimelineItemModel RegistrationOpens { get; set; }
    }
}
