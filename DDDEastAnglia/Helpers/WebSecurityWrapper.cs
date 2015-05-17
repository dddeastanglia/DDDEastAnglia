using WebMatrix.WebData;

namespace DDDEastAnglia.Helpers
{
    public class WebSecurityWrapper : IResetPasswordService
    {
        public string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow)
        {
            return WebSecurity.GeneratePasswordResetToken(username, tokenExpirationInMinutesFromNow);
        }

        public bool ResetPassword(string passwordResetToken, string newPassword)
        {
            return WebSecurity.ResetPassword(passwordResetToken, newPassword);
        }
    }
}