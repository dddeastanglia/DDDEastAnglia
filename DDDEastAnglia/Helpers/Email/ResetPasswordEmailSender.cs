using System;
using System.Net.Mail;
using DDDEastAnglia.Helpers.File;

namespace DDDEastAnglia.Helpers.Email
{
    public class ResetPasswordEmailSender
    {
        public const string FromEmailAddress = "admin@dddeastanglia.com";
        public const string FromEmailName = "DDD East Anglia";
        public const string ResetEmailSubject = "DDD East Anglia Password Reset Request";
        public const string ResetLinkToken = "[resetLink]";

        private readonly IEmailSender emailSender;
        private readonly IMessageFactory messageFactory;
        private readonly IFileContentsProvider fileContentsProvider;

        public ResetPasswordEmailSender(IEmailSender emailSender, IMessageFactory messageFactory, IFileContentsProvider fileContentsProvider)
        {
            if (emailSender == null)
            {
                throw new ArgumentNullException("emailSender");
            }
            
            if (messageFactory == null)
            {
                throw new ArgumentNullException("messageFactory");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }
            
            this.emailSender = emailSender;
            this.messageFactory = messageFactory;
            this.fileContentsProvider = fileContentsProvider;
        }

        public void SendEmail(string htmlTemplatePath, string textTemplatePath, string toAddress, string resetPasswordUrl)
        {
            string htmlTemplate = fileContentsProvider.GetFileContents(htmlTemplatePath);
            string textTemplate = fileContentsProvider.GetFileContents(textTemplatePath);

            var from = new MailAddress(FromEmailAddress, FromEmailName);
            var to = new MailAddress(toAddress);
            var html = htmlTemplate.Replace(ResetLinkToken, resetPasswordUrl);
            var text = textTemplate.Replace(ResetLinkToken, resetPasswordUrl);

            var message = messageFactory.Create(from, to, ResetEmailSubject, html, text);
            emailSender.Send(message);
        }
    }
}
