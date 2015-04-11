using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger.Email.Templates
{
    public class SessionCreatedMailTemplate : TokenSubstitutingMailTemplate
    {
        private const string SessionAbstractToken = "[SessionAbstract]";
        private const string SessionTitleToken = "[SessionTitle]";

        private SessionCreatedMailTemplate(string templateContent) : base(templateContent)
        {
        }

        public static IMailTemplate Create(string templatePath, Session session)
        {
            var template = new SessionCreatedMailTemplate(new FileContentsProvider().GetFileContents(templatePath));
            template.AddTokenSubstitution(SessionTitleToken, session.Title);
            template.AddTokenSubstitution(SessionAbstractToken, session.Abstract);

            return template;
        }
    }
}