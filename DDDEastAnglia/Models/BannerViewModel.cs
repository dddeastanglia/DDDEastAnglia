namespace DDDEastAnglia.Models
{
    public class BannerViewModel
    {
        public bool IsDuringSessionSubmission{get;set;}
        public bool IsDuringSessionVoting{get;set;}

        public string CurrentEventExpirationDate{get;set;}
    }
}