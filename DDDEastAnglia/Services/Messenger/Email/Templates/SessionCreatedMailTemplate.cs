using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger.Email.Templates
{
    public class SessionCreatedMailTemplate : TokenSubstitutingMailTemplate
    {
        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        private const string MailSubject = "DDD East Anglia Session Submission: " + SessionTitleToken;

        private SessionCreatedMailTemplate(string templateContent) : base(MailSubject, templateContent)
        {
        }

        public static IMailTemplate Create(Session session)
        {
            var template = new SessionCreatedMailTemplate(Emails.SessionSubmission);
            template.AddTokenSubstitution(SessionTitleToken, session.Title);
            template.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            return template;
        }
    }
}