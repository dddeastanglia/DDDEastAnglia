using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class GrokTalk
    {
        [Key]
        public int GrokTalkId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Abstract { get; set; }

        public UserProfile Speaker { get; set; }
    }
}