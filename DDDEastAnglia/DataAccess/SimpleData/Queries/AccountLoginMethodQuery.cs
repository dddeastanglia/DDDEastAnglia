using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Models;
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
                loginMethods.Add(new LoginMethod("dddea"));
            }
            
            List<string> oauthLoginProviders = db.webpages_OAuthMembership.FindAllByUserId(userId)
                                                 .Select(db.webpages_OAuthMembership.Provider).ToScalarList<string>();

            var oauthLogins = new[] {"github", "twitter", "google"}
                                .Where(oauthLoginProviders.Contains)
                                .Select(providerName => new LoginMethod(providerName));
            loginMethods.AddRange(oauthLogins);

            return loginMethods;
        }
    }
}
