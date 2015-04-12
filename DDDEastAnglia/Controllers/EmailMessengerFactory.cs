using DDDEastAnglia.Helpers;
using DDDEastAnglia.Services.Messenger.Email;
using System;

namespace DDDEastAnglia.Controllers
{
    public class EmailMessengerFactory
    {
        private readonly IPostman postman;

        public EmailMessengerFactory(IPostman postman)
        {
            if (postman == null)
            {
                throw new ArgumentNullException("postman");
            }

            this.postman = postman;
        }

        public EmailMessenger CreateEmailMessenger(IMailTemplate mailTemplate)
        {
            return new EmailMessenger(postman, mailTemplate);
        }
    }
}