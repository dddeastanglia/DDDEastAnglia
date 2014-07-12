using System;

namespace DDDEastAnglia.Models
{
    public class LoginMethod
    {
        public string ProviderName{get {return providerName;}}
        private readonly string providerName;

        public LoginMethod(string providerName)
        {
            if (providerName == null)
            {
                throw new ArgumentNullException("providerName");
            }
            
            this.providerName = providerName;
        }
    }
}
