using System;
using System.Collections.Generic;
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

            return externalLogins;
        }
    }
}
