using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email
{
    public interface IMessageFactory
    {
        IMailMessage Create(MailAddress from, MailAddress to, string subject, string htmlContent, string textContent);
    }
}