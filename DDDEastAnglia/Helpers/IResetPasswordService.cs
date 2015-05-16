namespace DDDEastAnglia.Helpers
{
    public interface IResetPasswordService
    {
        string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow);
        bool ResetPassword(string passwordResetToken, string newPassword);
    }
}
