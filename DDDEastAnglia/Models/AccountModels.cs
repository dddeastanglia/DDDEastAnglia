using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace DDDEastAnglia.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DDDEastAnglia")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }

        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "We have to have your email address or we can't contact you!")]
        [DisplayName("Email Address (not for public use, just so we can contact you")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This doesn't appear to be a valid email address")]
        public string EmailAddress { get; set; }
        [DataType(DataType.Url, ErrorMessage = "This doesn't appear to be a valid url")]
        [DisplayName("Website")]
        public string WebsiteUrl { get; set; }
        [DisplayName("Twitter Handle")]
        public string TwitterHandle { get; set; }
        public string Bio { get; set; }
        [DisplayName("Mobile Phone (not for public use, so we can contact you)")]
        public string MobilePhone { get; set; }
        [DisplayName("Are you a new speaker? (haven't spoken at a DDD before)")]
        public bool NewSpeaker { get; set; }

    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
