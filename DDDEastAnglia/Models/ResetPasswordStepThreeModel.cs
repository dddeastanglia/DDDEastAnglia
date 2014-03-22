using System.ComponentModel.DataAnnotations;
using CompareAttribute=System.Web.Mvc.CompareAttribute;

namespace DDDEastAnglia.Models
{
    public class ResetPasswordStepThreeModel
    {
        [Required]
        public string ResetToken { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "The new password must be at least {1} characters long.")]
        [DataType(DataType.Password)]
        [Display(Name = "new password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "confirm new password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation passwords do not match.")]
        public string ConfirmationPassword { get; set; }
    }
}