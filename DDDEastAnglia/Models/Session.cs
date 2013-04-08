using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class Session
    {
        [Key]
        public int SessionId { get; set; }
        
        [Required]
        [Display(Name = "title")]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "abstract")]
        public string Abstract { get; set; }

        [Display(Name = "submitted by")]
        public string SpeakerUserName { get; set; }

        public int Votes { get; set; }
    }
}