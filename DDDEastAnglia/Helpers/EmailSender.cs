using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using SendGridMail;
using SendGridMail.Transport;

namespace DDDEastAnglia.Helpers
{
    public class EmailSender
    {
        private readonly string smtpHost;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public EmailSender()
        {
            smtpHost = WebConfigurationManager.AppSettings["SMTPHost"];
            smtpPort = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);
            smtpUsername = WebConfigurationManager.AppSettings["SMTPUsername"];
            smtpPassword = WebConfigurationManager.AppSettings["SMTPPassword"];
        }

        public void SendPasswordResetEmail(string templatePath, string to, string resetPasswordUrl)
        {
            string template;

            using (var reader = new StreamReader(templatePath))
            {
                template = reader.ReadToEnd();
            }

            var fromAddress = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia");
            var toAddress = new[] { new MailAddress(to) };
            var subject = "DDD East Anglia Password Reset Request";
            var html = template.Replace("[resetLink]", resetPasswordUrl);
            var text = resetPasswordUrl;

            SendGrid message = SendGrid.GetInstance(fromAddress, toAddress, new MailAddress[0], new MailAddress[0], subject, html, text);
            var credentials = new NetworkCredential(smtpUsername, smtpPassword);
            SMTP.GetInstance(credentials, smtpHost, smtpPort).Deliver(message);
        }
    }
}