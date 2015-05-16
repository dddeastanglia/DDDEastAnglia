using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Services.Messenger.Email.Templates
{
    public static class PasswordResetMailTemplate
    {
        private const string PasswordResetUrlToken = "[resetLink]";

        private const string MailSubject = "DDD East Anglia Password Reset Request";

        public static IMailTemplate Create(string passwordResetUrl)
        {
            var template = new TokenSubstitutingMailTemplate(MailSubject, Emails.ResetPasswordRequest);
            template.AddTokenSubstitution(PasswordResetUrlToken, passwordResetUrl);

            return template;
        }
    }
}