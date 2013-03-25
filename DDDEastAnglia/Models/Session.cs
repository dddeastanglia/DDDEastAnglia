using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Abstract { get; set; }

        public string SpeakerUserName { get; set; }

        public int Votes { get; set; }
    }
}