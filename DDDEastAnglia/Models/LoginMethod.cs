using System;

namespace DDDEastAnglia.Models
{
    public class LoginMethod
    {
        public string ProviderName{get {return providerName;}}
        private readonly string providerName;

        public string DisplayName{get {return displayName;}}
        private readonly string displayName;

        public LoginMethod(string providerName, string displayName)
        {
            if (providerName == null)
            {
                throw new ArgumentNullException("providerName");
            }

            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");
            }
            
            this.providerName = providerName;
            this.displayName = displayName;
        }
    }
}
