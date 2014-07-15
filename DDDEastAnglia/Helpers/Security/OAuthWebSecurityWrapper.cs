using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia.Helpers.Security
{
    public class OAuthWebSecurityWrapper : IOAuthClientInfo
    {
        public ICollection<OAuthAccount> GetAccountsFromUserName(string userName)
        {
            return OAuthWebSecurity.GetAccountsFromUserName(userName);
        }

        public AuthenticationClientData GetOAuthClientData(string providerName)
        {
            return OAuthWebSecurity.GetOAuthClientData(providerName);
        }

        public ICollection<AuthenticationClientData> RegisteredClientData()
        {
            return OAuthWebSecurity.RegisteredClientData;
        }
    }
}
