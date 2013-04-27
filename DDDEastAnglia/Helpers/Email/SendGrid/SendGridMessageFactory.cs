using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email.SendGrid
{
    public class SendGridMessageFactory : IMessageFactory
    {
        public IMailMessage Create(MailAddress from, MailAddress to, string subject, string htmlContent, string textContent)
        {
            var sendGridMessage = SendGridMail.SendGrid.GetInstance(from, new[] {to}, new MailAddress[0], new MailAddress[0], subject, htmlContent, textContent);
            return new SendGridMessageWrapper(sendGridMessage);
        }
    }
}
