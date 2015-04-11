using System.Collections.Generic;

namespace DDDEastAnglia.Helpers
{
    public interface IMailTemplate
    {
        string Render(IDictionary<string, string> replacements);
    }
}