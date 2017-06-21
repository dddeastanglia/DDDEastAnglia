using System;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridMessageWrapper : IMailMessage
    {
        public SendGridMail.SendGrid SendGrid { get; }

        public MailAddress From => SendGrid.From;
        public MailAddress[] To => SendGrid.To;
        public string Subject => SendGrid.Subject;
        public string Html => SendGrid.Html;
        public string Text => SendGrid.Text;

        public SendGridMessageWrapper(SendGridMail.SendGrid sendGrid)
        {
            if (sendGrid == null)
            {
                throw new ArgumentNullException(nameof(sendGrid));
            }

            SendGrid = sendGrid;
        }
    }
}