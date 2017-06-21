using System.Security.Cryptography;
using System.Text;

namespace DDDEastAnglia.Helpers
{
    public class GravatarUrl
    {
        public string GetUrl(string userIdentifier, bool useIdenticon = false, int size = 50)
        {
            string defaultIconCode = useIdenticon ? "identicon" : "mm";

            if (string.IsNullOrWhiteSpace(userIdentifier))
            {
                // Deal with missing email addresses
                return string.Format("http://www.gravatar.com/avatar/0000?s={0}&d={1}&r=pg", size, defaultIconCode);
            }

            using (MD5 md5Hasher = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(userIdentifier));

                // Create a new Stringbuilder to collect the bytes and create a string
                var builder = new StringBuilder();

                // Loop through each byte of the hashed data and format each one as a hexadecimal string
                foreach (byte b in data)
                {
                    builder.Append(b.ToString("x2"));
                }

                return string.Format("http://www.gravatar.com/avatar/{0}?s={1}&d={2}&r=pg", builder, size, defaultIconCode);
            }
        }
    }
}