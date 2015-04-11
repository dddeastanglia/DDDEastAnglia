using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Models;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using MailMessage = DDDEastAnglia.Helpers.Email.MailMessage;

namespace DDDEastAnglia.Helpers
{
    public class SessionCreationMailMessenger : IMessenger<Session>
    {
        private readonly IPostman postman;
        private readonly IMailTemplate plainTextTemplate;
        private readonly IMailTemplate htmlTemplate;

        private const string sessionAbstractToken = "[SessionAbstract]";

        private const string sessionTitleToken = "[SessionTitle]";

        public SessionCreationMailMessenger(IPostman postman, IMailTemplate plainTextTemplate, IMailTemplate htmlTemplate)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            if (plainTextTemplate == null)
            {
                throw new ArgumentNullException("plainTextTemplate");
            }

            if (htmlTemplate == null)
            {
                throw new ArgumentNullException("htmlTemplate");
            }

            this.postman = postman;
            this.plainTextTemplate = plainTextTemplate;
            this.htmlTemplate = htmlTemplate;
        }

        public void Notify(UserProfile user, Session session)
        {
            var replacementTokens = new Dictionary<string, string>
            {
                {sessionAbstractToken, session.Abstract},
                {sessionTitleToken, session.Title}
            };
            MailMessage message = new MailMessage
            {
                To = new MailAddress(user.EmailAddress),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = "DDD East Anglia Session Submission: " + session.Title,
                Html = htmlTemplate.Render(replacementTokens),
                Text = plainTextTemplate.Render(replacementTokens)
            };

            postman.Deliver(message);
        }
    }
}