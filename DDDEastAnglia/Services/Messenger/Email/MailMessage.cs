using System.Net.Mail;

namespace DDDEastAnglia.Services.Messenger.Email
{
    public class MailMessage
    {
        public MailAddress From { get; set; }
        public MailAddress To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}