using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        
        [DisplayName("username")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "An email address must be specified")]
        [DisplayName("email address")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This does not appear to be a valid email address")]
        public string EmailAddress { get; set; }
        
        [DataType(DataType.Url, ErrorMessage = "This does not appear to be a valid url")]
        [DisplayName("website")]
        public string WebsiteUrl { get; set; }
        
        [DisplayName("twitter handle")]
        public string TwitterHandle { get; set; }
        
        public string Bio { get; set; }
        
        [DisplayName("mobile phone number")]
        public string MobilePhone { get; set; }
        
        [DisplayName("new speaker")]
        public bool NewSpeaker { get; set; }
         
        public string GravatarUrl { get; set; }

        [DisplayName("submitted sessions")]
        public int SubmittedSessionCount { get; set; }
    }
}