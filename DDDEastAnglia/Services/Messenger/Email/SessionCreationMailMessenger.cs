using System;
using System.Net.Mail;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger.Email
{
    public class SessionCreationMailMessenger : IMessenger<Session>
    {
        private readonly IPostman postman;
        private readonly IMailTemplate mailTemplate;

        public SessionCreationMailMessenger(IPostman postman, IMailTemplate mailTemplate)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            if (mailTemplate == null)
            {
                throw new ArgumentNullException("mailTemplate");
            }

            this.postman = postman;
            this.mailTemplate = mailTemplate;
        }

        public void Notify(UserProfile user, Session session)
        {
            MailMessage message = new MailMessage
            {
                To = new MailAddress(user.EmailAddress),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = "DDD East Anglia Session Submission: " + session.Title,
                Body = mailTemplate.Render()
            };

            postman.Deliver(message);
        }
    }
}