using SendGridMail.Transport;
using System;
using System.Net;
using System.Net.Mail;
using DDDEastAnglia.Services.Messenger.Email;
using MailMessage = DDDEastAnglia.Services.Messenger.Email.MailMessage;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridPostman : IPostman
    {
        private readonly IMailHostSettingsProvider hostSettingsProvider;
        private readonly IRenderer htmlRenderer;

        public SendGridPostman(IMailHostSettingsProvider hostSettingsProvider, IRenderer htmlRenderer)
        {
            if (hostSettingsProvider == null)
            {
                throw new ArgumentNullException("hostSettingsProvider");
            }

            if (htmlRenderer == null)
            {
                throw new ArgumentNullException("htmlRenderer");
            }

            this.hostSettingsProvider = hostSettingsProvider;
            this.htmlRenderer = htmlRenderer;
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
                htmlRenderer.Render(message.Body),
                message.Body);
            instance.Deliver(sendGrid);
        }
    }
}
