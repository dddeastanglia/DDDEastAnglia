using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Display(Name = "remember me")]
        public bool RememberMe { get; set; }
    }
}