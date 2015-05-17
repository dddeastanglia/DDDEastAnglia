using System;
using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public class TokenSubstitutingMailTemplate : IMailTemplate
    {
        private readonly string templateSubject;
        private readonly string templateContent;
        private readonly IDictionary<string, string> substitutions = new Dictionary<string, string>();

        public TokenSubstitutingMailTemplate(string templateSubject, string templateContent)
        {
            if (templateSubject == null)
            {
                throw new ArgumentNullException("templateSubject");
            }

            if (string.IsNullOrWhiteSpace(templateContent))
            {
                throw new ArgumentNullException("templateContent");
            }

            this.templateSubject = templateSubject;
            this.templateContent = templateContent;
        }

        public void AddTokenSubstitution(string token, string substitution)
        {
            substitutions[token] = substitution;
        }

        public string RenderBody()
        {
            return MakeSubstitutions(templateContent);
        }

        public string RenderSubjectLine()
        {
            return MakeSubstitutions(templateSubject);
        }

        private string MakeSubstitutions(string text)
        {
            var returnValue = text;
            foreach (var token in substitutions)
            {
                returnValue = returnValue.Replace(token.Key, token.Value);
            }

            return returnValue;
        }
    }
}