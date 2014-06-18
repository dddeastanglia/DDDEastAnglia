namespace DDDEastAnglia.Models
{
    public class TimelineModel
    {
        public string SubmissionOpens{get;set;}
        public string SubmissionCloses{get;set;}
        public string VotingOpens{get;set;}
        public string VotingCloses{get;set;}
        public string AgendaAnnounced{get;set;}
        public string RegistrationOpens{get;set;}

        public bool SubmissionPeriodPassed{get;set;}
        public bool VotingPeriodPassed{get;set;}
        public bool AgendaPeriodPassed{get;set;}
        public bool RegistrationPeriodPassed{get;set;}
    }
}
