using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using MarkdownSharp;
using System;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email
{
    public interface ISessionSubmissionMessageFactory
    {
        MailMessage Create(string htmlTemplatepath, string textTemplatePath,
            Session session,
            UserProfile speakerProfile, bool updated);
    }

    public class SessionSubmissionMessageFactory : ISessionSubmissionMessageFactory
    {
        public const string FromEmailAddress = "admin@dddeastanglia.com";
        public const string FromEmailName = "DDD East Anglia";
        private const string sessionAbstractToken = "[SessionAbstract]";
        private const string sessionTitleToken = "[SessionTitle]";

        private readonly IFileContentsProvider fileContentsProvider;

        public SessionSubmissionMessageFactory(IFileContentsProvider fileContentsProvider)
        {
            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            this.fileContentsProvider = fileContentsProvider;
        }

        public MailMessage Create(string htmlTemplatePath, string textTemplatePath, Session session, UserProfile speakerProfile, bool updated)
        {
            string htmlTemplate = fileContentsProvider.GetFileContents(htmlTemplatePath);
            string textTemplate = fileContentsProvider.GetFileContents(textTemplatePath);

            var from = new MailAddress(FromEmailAddress, FromEmailName);
            var to = new MailAddress(speakerProfile.EmailAddress, speakerProfile.Name);

            Markdown converter = new Markdown();

            var html = htmlTemplate
                            .Replace(sessionTitleToken, session.Title)
                            .Replace(sessionAbstractToken, converter.Transform(session.Abstract));
            var text = textTemplate
                            .Replace(sessionTitleToken, session.Title)
                            .Replace(sessionAbstractToken, session.Abstract);

            string emailSubject = updated ? "DDD East Anglia Updated Session: " + session.Title : "DDD East Anglia Session Submission: " + session.Title;
            return new MailMessage
            {
                From = from,
                To = to,
                Subject = emailSubject,
                Html = html,
                Text = text
            };
        }
    }
}