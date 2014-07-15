using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Helpers.Security;
using DDDEastAnglia.Models;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia.Helpers.LoginMethods
{
    public class ExternalLoginsProvider
    {
        private readonly IOAuthClientInfo oauthClientInfo;

        public ExternalLoginsProvider(IOAuthClientInfo oauthClientInfo)
        {
            if (oauthClientInfo == null)
            {
                throw new ArgumentNullException("oauthClientInfo");
            }
            
            this.oauthClientInfo = oauthClientInfo;
        }

        public IEnumerable<LoginMethod> GetForUser(string userName)
        {
            var accounts = oauthClientInfo.GetAccountsFromUserName(userName);
            var externalLogins = new List<LoginMethod>();

            foreach (OAuthAccount account in accounts)
            {
                var clientData = oauthClientInfo.GetOAuthClientData(account.Provider);

                externalLogins.Add(new LoginMethod
                {
                    DisplayName = clientData.DisplayName,
                    ProviderName = account.Provider,
                    ProviderUserId = account.ProviderUserId
                });
            }

            return externalLogins.OrderBy(l => l.DisplayName);
        }

        public IEnumerable<LoginMethod> GetAllAvailable()
        {
            var authenticationClientDatas = oauthClientInfo.RegisteredClientData();
            var externalLogins = new List<LoginMethod>();

            foreach (var externalLogin in authenticationClientDatas)
            {
                var provider = externalLogin.AuthenticationClient.ProviderName;

                externalLogins.Add(new LoginMethod
                {
                    DisplayName = externalLogin.DisplayName,
                    ProviderName = provider
                });
            }

            return externalLogins.OrderBy(l => l.DisplayName);
        }
    }
}
