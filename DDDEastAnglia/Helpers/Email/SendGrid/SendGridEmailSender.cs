using System;
using System.Net;
using SendGridMail.Transport;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IMailHostSettings hostSettings;

        public SendGridEmailSender(IMailHostSettings hostSettings)
        {
            if (hostSettings == null)
            {
                throw new ArgumentNullException("hostSettings");
            }
            
            this.hostSettings = hostSettings;
        }

        public void Send(IMailMessage message)
        {
            var credentials = new NetworkCredential(hostSettings.Username, hostSettings.Password);
            SMTP instance = SMTP.GetInstance(credentials, hostSettings.Host, hostSettings.Port);
            
            // don't like this, but will do for now
            var sendGridMessageWrapper = (SendGridMessageWrapper) message;
            instance.Deliver(sendGridMessageWrapper.SendGrid);
        }
    }
}