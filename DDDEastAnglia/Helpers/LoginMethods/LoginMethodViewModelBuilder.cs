using System;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.LoginMethods
{
    public class LoginMethodViewModelBuilder : IBuild<LoginMethod, LoginMethodViewModel>
    {
        private readonly LoginMethodIconProvider oauthLoginIconProvider;

        public LoginMethodViewModelBuilder(LoginMethodIconProvider oauthLoginIconProvider)
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
