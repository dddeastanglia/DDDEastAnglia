using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Helpers.Security;
using DDDEastAnglia.Models;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia.Helpers.LoginMethods
{
    public class ExternalLoginsDirectory
    {
        private readonly IOAuthClientInfo oAuthClientInfo;

        public ExternalLoginsDirectory(IOAuthClientInfo oAuthClientInfo)
        {
            if (oAuthClientInfo == null)
            {
                throw new ArgumentNullException("oAuthClientInfo");
            }

            this.oAuthClientInfo = oAuthClientInfo;
        }

        public IEnumerable<LoginMethod> GetForUser(string userName)
        {
            var accounts = oAuthClientInfo.GetAccountsFromUserName(userName);
            var externalLogins = new List<LoginMethod>();

            foreach (OAuthAccount account in accounts)
            {
                var clientData = oAuthClientInfo.GetOAuthClientData(account.Provider);

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
            var authenticationClientDatas = oAuthClientInfo.RegisteredClientData();
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
