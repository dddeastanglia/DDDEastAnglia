using System;

namespace DDDEastAnglia.Helpers.Email.Smtp
{
    public class SmtpHostSettings : IMailHostSettings
    {
        public const int DefaultSmtpPort = 25;

        public string Host{get {return smtpHost;}}
        private readonly string smtpHost;
        
        public int Port{get {return smtpPort;}}
        private readonly int smtpPort;
        
        public string Username{get {return smtpUsername;}}
        private readonly string smtpUsername;
        
        public string Password{get {return smtpPassword;}}
        private readonly string smtpPassword;

        public SmtpHostSettings(string smtpHost, int smtpPort = 25, string smtpUsername = null, string smtpPassword = null)
        {
            if (smtpHost == null)
            {
                throw new ArgumentNullException("smtpHost");
            }
            
            this.smtpHost = smtpHost;
            this.smtpPort = smtpPort;
            this.smtpUsername = smtpUsername;
            this.smtpPassword = smtpPassword;
        }
    }
}
