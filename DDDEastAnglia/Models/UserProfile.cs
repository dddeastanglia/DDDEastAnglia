using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Models
{
    public class UserProfile
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [DisplayName("username")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "We have to have your email address or we cannot contact you!")]
        [DisplayName("email address (will not be displayed publicly)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This does not appear to be a valid email address")]
        public string EmailAddress { get; set; }
        
        [DataType(DataType.Url, ErrorMessage = "This does not appear to be a valid url")]
        [DisplayName("website")]
        public string WebsiteUrl { get; set; }
        
        [DisplayName("twitter handle")]
        public string TwitterHandle { get; set; }
        
        public string Bio { get; set; }
        
        [DisplayName("mobile phone number (will not be displayed publicly)")]
        public string MobilePhone { get; set; }
        
        [DisplayName("new speaker (have not spoken at a DDD event before)")]
        public bool NewSpeaker { get; set; }

        public DateTimeOffset? JoinedAt { get; set; }

        public string GravatarUrl(int size = 50)
        {
            return new GravatarUrl().GetUrl(EmailAddress, size: size);
        }
    }
}
