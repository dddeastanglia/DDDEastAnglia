using System;
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
            return new TestMailMessage
            {
                To = new MailAddress(userProfile.EmailAddress, userProfile.Name),
                From = new MailAddress("admin@dddeastanglia.com", "DDD East Anglia"),
                Subject = mailTemplate.RenderSubjectLine(),
                Body = mailTemplate.RenderBody()
            };
        }
    }

    internal class TestMailMessage : Site.MailMessage
    {
        public override bool Equals(object obj)
        {
            return Equals(this, obj as Site.MailMessage);
        }

        private static bool Equals(Site.MailMessage x, Site.MailMessage y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return Equals(x.From, y.From)
                   && Equals(x.To, y.To)
                   && String.Equals(x.Subject, y.Subject)
                   && String.Equals(x.Body, y.Body);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 17;
                hashCode = hashCode * 19 + (From != null ? From.GetHashCode() : 0);
                hashCode = hashCode * 19 + (To != null ? To.GetHashCode() : 0);
                hashCode = hashCode * 19 + (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = hashCode * 19 + (Body != null ? Body.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
