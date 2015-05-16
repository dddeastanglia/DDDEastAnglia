using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Services.Messenger.Email.Templates
{
    public class PasswordResetMailTemplate : TokenSubstitutingMailTemplate
    {
        private const string PasswordResetUrlToken = "[resetLink]";

        private const string MailSubject = "DDD East Anglia Password Reset Request";

        private PasswordResetMailTemplate(string templateContent)
            : base(MailSubject, templateContent)
        {
        }

        public static IMailTemplate Create(string passwordResetUrl)
        {
            var template = new PasswordResetMailTemplate(Emails.ResetPasswordRequest);
            template.AddTokenSubstitution(PasswordResetUrlToken, passwordResetUrl);

            return template;
        }
    }
}