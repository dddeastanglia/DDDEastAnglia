using System.Net.Mail;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using Site = DDDEastAnglia.Services.Messenger.Email;

namespace DDDEastAnglia.Tests.Helpers.Email
{
    internal static class MailMessage
    {
        internal static Site.MailMessage FromTemplate(IMailTemplate mailTemplate, UserProfile userProfile)
        {
            return new Site.MailMessage
            {
                To = new MailAddress(userProfile.EmailAddress, userProfile.Name),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = mailTemplate.RenderSubjectLine(),
                Body = mailTemplate.RenderBody()
            };
        }
    }
}
