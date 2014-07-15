using System.Collections.Generic;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia.Helpers.Security
{
    public interface IOAuthClientInfo
    {
        ICollection<OAuthAccount> GetAccountsFromUserName(string userName);
        AuthenticationClientData GetOAuthClientData(string providerName);
        ICollection<AuthenticationClientData> RegisteredClientData();
    }
}
