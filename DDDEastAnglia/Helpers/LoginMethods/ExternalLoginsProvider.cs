using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia.Helpers.LoginMethods
{
    public class ExternalLoginsProvider
    {
        private readonly LoginMethodIconProvider oauthLoginIconProvider;

        public ExternalLoginsProvider(LoginMethodIconProvider oauthLoginIconProvider)
        {
            if (oauthLoginIconProvider == null)
            {
                throw new ArgumentNullException("oauthLoginIconProvider");
            }
            
            this.oauthLoginIconProvider = oauthLoginIconProvider;
        }

        public IEnumerable<LoginMethodViewModel> GetForUser(string userName)
        {
            var accounts = OAuthWebSecurity.GetAccountsFromUserName(userName);
            var externalLogins = new List<LoginMethodViewModel>();

            foreach (OAuthAccount account in accounts)
            {
                var clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

                externalLogins.Add(new LoginMethodViewModel
                {
                    Name = clientData.DisplayName,
                    ProviderName = account.Provider,
                    ProviderUserId = account.ProviderUserId,
                    Icon = oauthLoginIconProvider.GetIcon(account.Provider)
                });
            }

            return externalLogins.OrderBy(l => l.Name);
        }

        public IEnumerable<LoginMethodViewModel> GetAllAvailable()
        {
            var authenticationClientDatas = OAuthWebSecurity.RegisteredClientData;
            var externalLogins = new List<LoginMethodViewModel>();

            foreach (var externalLogin in authenticationClientDatas)
            {
                var provider = externalLogin.AuthenticationClient.ProviderName;

                externalLogins.Add(new LoginMethodViewModel
                {
                    Name = externalLogin.DisplayName,
                    ProviderName = provider,
                    Icon = oauthLoginIconProvider.GetIcon(provider)
                });
            }

            return externalLogins.OrderBy(l => l.Name);
        }
    }
}
