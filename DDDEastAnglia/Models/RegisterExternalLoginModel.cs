using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "full name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The email address field is required.")]
        [Display(Name = "email address (will not be displayed publicly)")]
        public string EmailAddress { get; set; }

        public string ExternalLoginData { get; set; }
    }
}