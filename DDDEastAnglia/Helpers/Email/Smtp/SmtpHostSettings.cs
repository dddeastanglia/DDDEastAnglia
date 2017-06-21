using System;

namespace DDDEastAnglia.Helpers.Email.Smtp
{
    public class SmtpHostSettings : IMailHostSettings
    {
        public const int DefaultSmtpPort = 25;

        public string Host { get; }

        public int Port { get; }

        public string Username { get; }

        public string Password { get; }

        public SmtpHostSettings(string smtpHost, int smtpPort = 25, string smtpUsername = null, string smtpPassword = null)
        {
            if (smtpHost == null)
            {
                throw new ArgumentNullException("smtpHost");
            }

            Host = smtpHost;
            Port = smtpPort;
            Username = smtpUsername;
            Password = smtpPassword;
        }
    }
}
