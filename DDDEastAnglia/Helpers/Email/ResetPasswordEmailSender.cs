using DDDEastAnglia.Helpers.File;
using System;
using System.Net.Mail;
using DDDEastAnglia.Services.Messenger.Email;
using MailMessage = DDDEastAnglia.Services.Messenger.Email.MailMessage;

namespace DDDEastAnglia.Helpers.Email
{
    public interface IResetPasswordEmailSender
    {
        void SendEmail(string htmlTemplatePath, string textTemplatePath,
            string toAddress, string resetPasswordUrl);
    }

    public class ResetPasswordEmailSender : IResetPasswordEmailSender
    {
        public const string FromEmailAddress = "admin@dddeastanglia.com";
        public const string FromEmailName = "DDD East Anglia";
        public const string ResetEmailSubject = "DDD East Anglia Password Reset Request";
        public const string ResetLinkToken = "[resetLink]";

        private readonly IPostman postman;
        private readonly IFileContentsProvider fileContentsProvider;

        public ResetPasswordEmailSender(IPostman postman, IFileContentsProvider fileContentsProvider)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            this.postman = postman;
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

            var message = new MailMessage() { From = from, To = to, Subject = ResetEmailSubject, Body = text };
            postman.Deliver(message);
        }
    }
}
