using System;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public class LoginMethodViewModelBuilder : IBuild<LoginMethod, LoginMethodViewModel>
    {
        private readonly OauthLoginIconProvider oauthLoginIconProvider;

        public LoginMethodViewModelBuilder(OauthLoginIconProvider oauthLoginIconProvider)
        {
            if (oauthLoginIconProvider == null)
            {
                throw new ArgumentNullException("oauthLoginIconProvider");
            }
            
            this.oauthLoginIconProvider = oauthLoginIconProvider;
        }

        public LoginMethodViewModel Build(LoginMethod loginMethod)
        {
            return new LoginMethodViewModel
            {
                Name = loginMethod.DisplayName,
                ProviderName = loginMethod.ProviderName,
                ProviderUserId = loginMethod.ProviderUserId,
                Icon = oauthLoginIconProvider.GetIcon(loginMethod.ProviderName)
            };
        }
    }
}
