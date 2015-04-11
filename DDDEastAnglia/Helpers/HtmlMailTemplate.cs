using MarkdownSharp;
using System;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    internal class HtmlMailTemplate : IMailTemplate
    {
        private readonly IMailTemplate mailTemplate;
        private readonly string htmlTemplate;

        public HtmlMailTemplate(IMailTemplate mailTemplate, string htmlTemplate)
        {
            if (mailTemplate == null)
            {
                throw new ArgumentNullException("mailTemplate");
            }

            if (htmlTemplate == null)
            {
                throw new ArgumentNullException("htmlTemplatePath");
            }

            this.mailTemplate = mailTemplate;
            this.htmlTemplate = htmlTemplate;
        }

        public string Render(IDictionary<string, string> replacements)
        {
            var messsageContent = mailTemplate.Render(replacements);
            messsageContent = new Markdown().Transform(messsageContent);
            var htmlMessage = htmlTemplate.Replace("[MessageBody]", messsageContent);

            return htmlMessage;
        }
    }
}