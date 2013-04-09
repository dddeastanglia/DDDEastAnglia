using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DDDEastAnglia.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "full name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "email address")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The password must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}