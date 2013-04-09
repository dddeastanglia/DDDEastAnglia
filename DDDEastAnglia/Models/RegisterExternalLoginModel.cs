using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }
}