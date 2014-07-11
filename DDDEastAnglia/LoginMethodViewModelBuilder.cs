using System;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.Models;

namespace DDDEastAnglia
{
    public class LoginMethodViewModelBuilder : IBuild<LoginMethod, LoginMethodViewModel>
    {
        public LoginMethodViewModel Build(LoginMethod loginMethod)
        {
            switch (loginMethod.ProviderName)
            {
                case "dddea":
                    return new LoginMethodViewModel { Name = "DDDEA Account", Icon = "icon-user" };
                case "github":
                    return new LoginMethodViewModel { Name = "GitHub", Icon = "icon-github" };
                case "twitter":
                    return new LoginMethodViewModel { Name = "Twitter", Icon = "icon-twitter" };
                case "google":
                    return new LoginMethodViewModel { Name = "Google", Icon = "icon-google-plus" };
                default:
                    string message = string.Format("Unknown login method '{0}'", loginMethod.ProviderName);
                    throw new NotSupportedException(message);
            }
        }
    }
}
