using System.Collections.Generic;
using DotNetOpenAuth.AspNet;
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

        public bool HasLocalAccount(int getUserId)
        {
            return OAuthWebSecurity.HasLocalAccount(getUserId);
        }

        public string GetUserName(string provider, string providerUserId)
        {
            return OAuthWebSecurity.GetUserName(provider, providerUserId);
        }

        public void DeleteAccount(string provider, string providerUserId)
        {
            OAuthWebSecurity.DeleteAccount(provider, providerUserId);
        }

        public AuthenticationResult VerifyAuthentication(string action)
        {
            return OAuthWebSecurity.VerifyAuthentication(action);
        }

        public bool Login(string provider, string providerUserId, bool createPersistentCookie)
        {
            return OAuthWebSecurity.Login(provider, providerUserId, createPersistentCookie);
        }

        public void CreateOrUpdateAccount(string provider, string providerUserId, string name)
        {
            OAuthWebSecurity.CreateOrUpdateAccount(provider, providerUserId, name);
        }

        public string SerializeProviderUserId(string provider, string providerUserId)
        {
            return OAuthWebSecurity.SerializeProviderUserId(provider, providerUserId);
        }

        public bool TryDeserializeProviderUserId(string externalLoginData, out string provider, out string providerUserId)
        {
            return OAuthWebSecurity.TryDeserializeProviderUserId(externalLoginData, out provider, out providerUserId);
        }
    }
}
