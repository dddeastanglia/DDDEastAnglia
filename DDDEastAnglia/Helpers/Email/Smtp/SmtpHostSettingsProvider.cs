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

        private readonly string host;
        private readonly int port;
        private readonly string username;
        private readonly string password;

        public SmtpHostSettingsProvider(IAppSettingsProvider appSettingsProvider)
        {
            if (appSettingsProvider == null)
            {
                throw new ArgumentNullException("appSettingsProvider");
            }

            host = appSettingsProvider.GetSetting(SmtpHostKey);
            port = int.Parse(appSettingsProvider.GetSetting(SmtpPortKey));
            username = appSettingsProvider.GetSetting(SmtpUsernameKey);
            password = appSettingsProvider.GetSetting(SmtpPasswordKey);
        }

        public IMailHostSettings GetSettings()
        {
            return new SmtpHostSettings(host, port, username, password);
        }
    }
}