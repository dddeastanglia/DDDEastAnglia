using System;

namespace DDDEastAnglia.Models
{
    public class LoginMethod
    {
        public string ProviderName { get; }

        public LoginMethod(string providerName)
        {
            if (providerName == null)
            {
                throw new ArgumentNullException(nameof(providerName));
            }

            ProviderName = providerName;
        }
    }
}
