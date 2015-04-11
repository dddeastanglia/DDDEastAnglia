using System.Net.Mail;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email;
using NSubstitute;
using NUnit.Framework;
using MailMessage = DDDEastAnglia.Services.Messenger.Email.MailMessage;

namespace DDDEastAnglia.Tests.Services.Messengers.Email
{
    [TestFixture]
    public class SessionCreatedMailMessengerFacts
    {
        [TestFixture]
        public class Notify_Should
        {
            [Test]
            public void Prepare_The_Mail_For_Delivery()
            {
                var postman = Substitute.For<IPostman>();
                var template = Substitute.For<IMailTemplate>();
                template.Render().Returns("Message body");

                var messenger = new SessionCreationMailMessenger(postman, template);
                messenger.Notify(new UserProfile {EmailAddress = "speaker@dddeastanglia.com"},
                    new Session {Title = "My awesome session", Abstract = ""});

                var message = new MailMessage
                {
                    From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                    To = new MailAddress("speaker@dddeastanglia.com"),
                    Subject = "DDD East Anglia Session Submission: My awesome session",
                    Body = "Message body"
                };

                postman.Received(1).Deliver(message);
            }
        }
    }
}
