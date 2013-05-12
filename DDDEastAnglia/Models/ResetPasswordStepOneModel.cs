using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Models
{
    public class ResetPasswordStepOneModel
    {
        [Display(Name = "user name")]
        public string UserName { get; set; } 
        
        [Display(Name = "email address")]
        public string EmailAddress { get; set; } 
    }
}