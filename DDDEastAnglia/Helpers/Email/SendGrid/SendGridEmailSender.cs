using System;
using System.Net;
using SendGridMail.Transport;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IMailHostSettingsProvider hostSettingsProvider;

        public SendGridEmailSender(IMailHostSettingsProvider hostSettingsProvider)
        {
            if (hostSettingsProvider == null)
            {
                throw new ArgumentNullException("hostSettingsProvider");
            }

            this.hostSettingsProvider = hostSettingsProvider;
        }

        public void Send(IMailMessage message)
        {
            var hostSettings = hostSettingsProvider.GetSettings();
            var credentials = new NetworkCredential(hostSettings.Username, hostSettings.Password);
            SMTP instance = SMTP.GetInstance(credentials, hostSettings.Host, hostSettings.Port);
            
            // don't like this, but will do for now
            var sendGridMessageWrapper = (SendGridMessageWrapper) message;
            instance.Deliver(sendGridMessageWrapper.SendGrid);
        }
    }
}
