using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class ResetPasswordStepTwoModel
    {
        [Required]
        public string Token { get; set; } 
    }
}
