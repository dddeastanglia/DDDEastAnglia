namespace DDDEastAnglia.Helpers
{
    public interface IResetPasswordThingy
    {
        string GeneratePasswordResetToken(string username, int tokenExpirationInMinutesFromNow);
        bool ResetPassword(string passwordResetToken, string newPassword);
    }
}
