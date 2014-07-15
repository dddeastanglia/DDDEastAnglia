using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;
using Microsoft.Web.WebPages.OAuth;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class AccountLoginMethodQuery : IAccountLoginMethodQuery
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public IEnumerable<LoginMethod> GetLoginMethods(int userId)
        {
            var loginMethods = new List<LoginMethod>();
            var dddeaLogin = db.webpages_Membership.FindByUserId(userId);

            if (dddeaLogin != null)
            {
                loginMethods.Add(new LoginMethod("dddea", "DDDEA Account"));
            }
            
            List<string> oauthLoginProviders = db.webpages_OAuthMembership.FindAllByUserId(userId)
                                                 .Select(db.webpages_OAuthMembership.Provider).ToScalarList<string>();

            var availableOauthProviders = OAuthWebSecurity.RegisteredClientData;
            var oauthLogins = availableOauthProviders.Where(p => oauthLoginProviders.Contains(p.AuthenticationClient.ProviderName))
                                                     .Select(p => new LoginMethod(p.AuthenticationClient.ProviderName, p.DisplayName));
            loginMethods.AddRange(oauthLogins);

            return loginMethods;
        }
    }
}
