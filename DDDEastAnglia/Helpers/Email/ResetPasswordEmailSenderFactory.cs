using DDDEastAnglia.Helpers.AppSettings;
using DDDEastAnglia.Helpers.Email.SendGrid;
using DDDEastAnglia.Helpers.Email.Smtp;
using DDDEastAnglia.Helpers.File;

namespace DDDEastAnglia.Helpers.Email
{
    // can be removed once Ninject is present
    public static class ResetPasswordEmailSenderFactory
    {
        public static ResetPasswordEmailSender Create()
        {
            var hostSettingsProvider = new SmtpHostSettingsProvider(new WebConfigurationAppSettingsProvider());
            var emailSender = new SendGridEmailSender(hostSettingsProvider.GetSettings());
            var resetPasswordEmailSender = new ResetPasswordEmailSender(emailSender, new SendGridMessageFactory(), new FileContentsProvider());
            return resetPasswordEmailSender;
        }
    }
}
