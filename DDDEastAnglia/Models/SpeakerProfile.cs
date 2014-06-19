using System.ComponentModel.DataAnnotations;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Models
{
    public class SpeakerProfile
    {
        [Key]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }
        
        public string EmailAddress { get; set; }
        
        public bool NewSpeaker { get; set; }

        public int NumberOfSubmittedSessions { get; set; }

        public string GravatarUrl()
        {
            return new GravatarUrl().GetUrl(EmailAddress);
        }
    }
}
