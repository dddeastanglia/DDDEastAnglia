using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public class LoginMethodViewModelBuilder : IBuild<LoginMethod, LoginMethodViewModel>
    {
        public LoginMethodViewModel Build(LoginMethod loginMethod)
        {
            var viewModel = new LoginMethodViewModel {Name = loginMethod.DisplayName};
            
            switch (loginMethod.ProviderName)
            {
                case "dddea":
                    viewModel.Icon = "icon-user";
                    break;
                case "github":
                    viewModel.Icon = "icon-github";
                    break;
                case "twitter":
                    viewModel.Icon = "icon-twitter";
                    break;
                case "google":
                    viewModel.Icon = "icon-google-plus";
                    break;
                default:
                    viewModel.Icon = "icon-question-sign";
                    break;
            }

            return viewModel;
        }
    }
}
