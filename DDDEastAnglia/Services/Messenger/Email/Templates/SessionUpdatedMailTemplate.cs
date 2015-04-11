using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.File;
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

        public static IMailTemplate Create(string templatePath, Session session)
        {
            var template = new SessionUpdatedMailTemplate(new FileContentsProvider().GetFileContents(templatePath));
            template.AddTokenSubstitution(SessionTitleToken, session.Title);
            template.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            return template;
        }
    }
}