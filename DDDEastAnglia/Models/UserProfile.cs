using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace DDDEastAnglia.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        [DisplayName("username")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "We have to have your email address or we cannot contact you!")]
        [DisplayName("email address (will not be displayed publicly)")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This does not appear to be a valid email address")]
        public string EmailAddress { get; set; }
        
        [DataType(DataType.Url, ErrorMessage = "This does not appear to be a valid url")]
        [DisplayName("website")]
        public string WebsiteUrl { get; set; }
        
        [DisplayName("twitter handle")]
        public string TwitterHandle { get; set; }
        
        public string Bio { get; set; }
        
        [DisplayName("mobile phone number (will not be displayed publicly)")]
        public string MobilePhone { get; set; }
        
        [DisplayName("I am a new speaker (have not spoken at a DDD event before)")]
        public bool NewSpeaker { get; set; }

        public string GravatarUrl(int size = 50)
        {
            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                // deal with missing email addresses
                return string.Format("http://www.gravatar.com/avatar/0000?s={0}&d=mm&r=pg", size);
            }

            using (MD5 md5Hasher = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.  
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(EmailAddress));

                // Create a new Stringbuilder to collect the bytes  
                // and create a string.  
                var builder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string.  
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }

                return string.Format("http://www.gravatar.com/avatar/{0}?s={1}&d=mm&r=pg", builder, size);
            }
        }
    }
}
