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
    public class SessionSubmittedMailMessengerFacts
    {
        [TestFixture]
        public class Notify_Should
        {
            [Test]
            public void Prepare_The_Mail_For_Delivery()
            {
                var postman = Substitute.For<IPostman>();
                var template = Substitute.For<IMailTemplate>();
                template.RenderBody().Returns("Message body");
                template.RenderSubjectLine().Returns("Message subject");

                var messenger = new EmailMessenger(postman, template);
                messenger.Notify(new UserProfile {EmailAddress = "speaker@dddeastanglia.com"});

                var message = new MailMessage
                {
                    From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                    To = new MailAddress("speaker@dddeastanglia.com"),
                    Subject = "Message subject",
                    Body = "Message body"
                };

                postman.Received(1).Deliver(message);
            }
        }
    }
}
