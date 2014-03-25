using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DDDEastAnglia.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The email address field is required.")]
        [Display(Name = "email address (will not be displayed publicly)")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "username")]
        public string UserName { get; set; }

        [Required]
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