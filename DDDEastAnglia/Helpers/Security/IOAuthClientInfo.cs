using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia.Helpers.Security
{
    public interface IOAuthClientInfo
    {
        ICollection<OAuthAccount> GetAccountsFromUserName(string userName);
        AuthenticationClientData GetOAuthClientData(string providerName);
        ICollection<AuthenticationClientData> RegisteredClientData();
        bool HasLocalAccount(int getUserId);
        string GetUserName(string provider, string providerUserId);
        void DeleteAccount(string provider, string providerUserId);
        AuthenticationResult VerifyAuthentication(string action);
        bool Login(string provider, string providerUserId, bool createPersistentCookie);
        void CreateOrUpdateAccount(string provider, string providerUserId, string name);
        string SerializeProviderUserId(string provider, string providerUserId);
        bool TryDeserializeProviderUserId(string externalLoginData, out string provider, out string providerUserId);
    }
}
