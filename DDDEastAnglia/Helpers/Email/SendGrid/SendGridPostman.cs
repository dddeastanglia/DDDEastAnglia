using SendGridMail.Transport;
using System;
using System.Net;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridPostman : IPostman
    {
        private readonly IMailHostSettingsProvider hostSettingsProvider;

        public SendGridPostman(IMailHostSettingsProvider hostSettingsProvider)
        {
            if (hostSettingsProvider == null)
            {
                throw new ArgumentNullException("hostSettingsProvider");
            }

            this.hostSettingsProvider = hostSettingsProvider;
        }

        public void Deliver(MailMessage message)
        {
            var hostSettings = hostSettingsProvider.GetSettings();
            var credentials = new NetworkCredential(hostSettings.Username, hostSettings.Password);
            SMTP instance = SMTP.GetInstance(credentials, hostSettings.Host, hostSettings.Port);

            var sendGrid = SendGridMail.SendGrid.GetInstance(
                message.From,
                new[] { message.To },
                new MailAddress[0],
                new MailAddress[0],
                message.Subject,
                message.Html,
                message.Text);
            instance.Deliver(sendGrid);
        }
    }
}
