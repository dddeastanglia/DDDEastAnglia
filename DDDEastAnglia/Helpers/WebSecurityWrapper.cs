using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace DDDEastAnglia.Helpers
{
    public class WebSecurityWrapper : IResetPasswordThingy
    {
        public string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow)
        {
            EnsureAccountIsConfirmed(username);
            return WebSecurity.GeneratePasswordResetToken(username, tokenExpirationInMinutesFromNow);
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);
        }

        private void EnsureAccountIsConfirmed(string username)
        {
            var user = Membership.GetUser(username);
            int userId = (int) user.ProviderUserKey;

            var connectionString = ConfigurationManager.ConnectionStrings["DDDEastAnglia"].ConnectionString;
            var newPassword = GeneratePassword();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $@"
IF NOT EXISTS (SELECT [UserID] FROM [dbo].[webpages_Membership] WHERE [UserId] = {userId})
	INSERT INTO [dbo].[webpages_Membership] ([UserId], [Password], [PasswordSalt], [IsConfirmed]) VALUES ({userId}, '{newPassword}', '', 1)
";
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GeneratePassword()
        {
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var numArray = new byte[16];
                cryptoServiceProvider.GetBytes(numArray);
                return HttpServerUtility.UrlTokenEncode(numArray);
            }
        }
    }
}
