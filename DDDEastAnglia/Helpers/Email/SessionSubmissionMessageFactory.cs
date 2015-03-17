using System.Security.Cryptography.X509Certificates;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using Glimpse.Mvc.AlternateType;
using MarkdownSharp;
using System;
using System.Net.Mail;

namespace DDDEastAnglia.Helpers.Email
{
    public interface ISessionSubmissionMessageFactory
    {
        IMailMessage Create(string htmlTemplatepath, string textTemplatePath, Session session,
            UserProfile speakerProfile, bool updated);
    }

    public class SessionSubmissionMessageFactory : ISessionSubmissionMessageFactory
    {
        public const string FromEmailAddress = "admin@dddeastanglia.com";
        public const string FromEmailName = "DDD East Anglia";
        private const string sessionAbstractToken = "[SessionAbstract]";
        private const string sessionTitleToken = "[SessionTitle]";

        private readonly IMessageFactory messageFactory;
        private readonly IFileContentsProvider fileContentsProvider;

        public SessionSubmissionMessageFactory(IMessageFactory messageFactory, IFileContentsProvider fileContentsProvider)
        {
            if (messageFactory == null)
            {
                throw new ArgumentNullException("messageFactory");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            this.messageFactory = messageFactory;
            this.fileContentsProvider = fileContentsProvider;
        }

        public IMailMessage Create(string htmlTemplatePath, string textTemplatePath, Session session, UserProfile speakerProfile, bool updated)
        {
            string htmlTemplate = fileContentsProvider.GetFileContents(htmlTemplatePath);
            string textTemplate = fileContentsProvider.GetFileContents(textTemplatePath);

            var from = new MailAddress(FromEmailAddress, FromEmailName);
            var to = new MailAddress(speakerProfile.EmailAddress, speakerProfile.Name);

            Markdown converter = new Markdown();

            var html = htmlTemplate.Replace(sessionTitleToken, session.Title).Replace(sessionAbstractToken, converter.Transform(session.Abstract));

            var text = textTemplate.Replace(sessionTitleToken, session.Title).Replace(sessionAbstractToken, session.Abstract);

            string emailSubject = updated ? "DDD East Anglia Updated Session: " + session.Title : "DDD East Anglia Session Submission: " + session.Title;
            return messageFactory.Create(from, to, emailSubject, html, text);
        }
    }
}