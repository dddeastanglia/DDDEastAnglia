using System;
using DDDEastAnglia.Helpers.AppSettings;

namespace DDDEastAnglia.Helpers.Email.Smtp
{
    public class SmtpHostSettingsProvider : IMailHostSettingsProvider
    {
        public const string SmtpHostKey = "SMTPHost";
        public const string SmtpPortKey = "SMTPPort";
        public const string SmtpUsernameKey = "SMTPUsername";
        public const string SmtpPasswordKey = "SMTPPassword";

        private readonly SmtpHostSettings smtpHostSettings;

        public SmtpHostSettingsProvider(IAppSettingsProvider appSettingsProvider)
        {
            if (appSettingsProvider == null)
            {
                throw new ArgumentNullException(nameof(appSettingsProvider));
            }

            string host = appSettingsProvider.GetSetting(SmtpHostKey);
            int port = int.Parse(appSettingsProvider.GetSetting(SmtpPortKey));
            string username = appSettingsProvider.GetSetting(SmtpUsernameKey);
            string password = appSettingsProvider.GetSetting(SmtpPasswordKey);
            smtpHostSettings = new SmtpHostSettings(host, port, username, password);
        }

        public IMailHostSettings GetSettings()
        {
            return smtpHostSettings;
        }
    }
}
