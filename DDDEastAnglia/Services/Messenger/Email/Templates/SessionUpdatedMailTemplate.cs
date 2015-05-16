using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger.Email.Templates
{
    public class SessionUpdatedMailTemplate : TokenSubstitutingMailTemplate
    {
        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        private const string MailSubject = "DDD East Anglia Updated Session: " + SessionTitleToken;

        private SessionUpdatedMailTemplate(string templateContent) : base(MailSubject, templateContent)
        {
        }

        public static IMailTemplate Create(Session session)
        {
            var template = new SessionUpdatedMailTemplate(Emails.SessionSubmission);
            template.AddTokenSubstitution(SessionTitleToken, session.Title);
            template.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            return template;
        }
    }
}