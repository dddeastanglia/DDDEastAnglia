using System;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public abstract class TokenSubstitutingMailTemplate : IMailTemplate
    {
        private readonly string templateContent;
        private readonly IDictionary<string, string> substitutions = new Dictionary<string, string>();

        protected TokenSubstitutingMailTemplate(string templateContent)
        {
            if (string.IsNullOrWhiteSpace(templateContent))
            {
                throw new ArgumentNullException("templateContent");
            }

            this.templateContent = templateContent;
        }

        protected void AddTokenSubstitution(string token, string substitution)
        {
            substitutions[token] = substitution;
        }

        public string Render()
        {
            var content = templateContent;
            foreach (var token in substitutions)
            {
                content = content.Replace(token.Key, token.Value);
            }

            return content;
        }
    }
}