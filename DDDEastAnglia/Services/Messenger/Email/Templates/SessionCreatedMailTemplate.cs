using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger.Email.Templates
{
    public static class SessionCreatedMailTemplate
    {
        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        private const string MailSubject = "DDD East Anglia Session Submission: " + SessionTitleToken;

        public static IMailTemplate Create(Session session)
        {
            var template = new TokenSubstitutingMailTemplate(MailSubject, Emails.SessionSubmission);
            template.AddTokenSubstitution(SessionTitleToken, session.Title);
            template.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            return template;
        }
    }
}