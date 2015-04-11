using System;
using System.Collections.Generic;
using DDDEastAnglia.Helpers.File;

namespace DDDEastAnglia.Helpers
{
    internal class TokenSubstitutingMailTemplate : IMailTemplate
    {
        private readonly string templatePath;
        private readonly IFileContentsProvider fileContentsProvider;

        public TokenSubstitutingMailTemplate(string templatePath, IFileContentsProvider fileContentsProvider)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
            {
                throw new ArgumentNullException("templatePath");
            }

            if (fileContentsProvider == null)
            {
                throw new ArgumentNullException("fileContentsProvider");
            }

            this.templatePath = templatePath;
            this.fileContentsProvider = fileContentsProvider;
        }

        public string Render(IDictionary<string, string> replacements)
        {
            var content = fileContentsProvider.GetFileContents(templatePath);

            foreach (var token in replacements)
            {
                content = content.Replace(token.Key, token.Value);
            }

            return content;
        }
    }
}