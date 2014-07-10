using System.Collections.Generic;
using DDDEastAnglia.Models;
using Simple.Data;

namespace DDDEastAnglia.DataAccess.SimpleData.Queries
{
    public class AccountLoginMethodQuery : IAccountLoginMethodQuery
    {
        private readonly dynamic db = Database.OpenNamedConnection("DDDEastAnglia");

        public LoginMethods GetLoginMethods(int userId)
        {
            var dddeaLogin = db.webpages_Membership.FindByUserId(userId);
            bool hasDddeaLogin = dddeaLogin != null;
            
            List<string> oauthLogins = db.webpages_OAuthMembership.FindAllByUserId(userId)
                                         .Select(db.webpages_OAuthMembership.Provider).ToScalarList<string>();
            bool hasGitHubLogin = oauthLogins.Contains("github");
            bool hasTwitterLogin = oauthLogins.Contains("twitter");
            bool hasGoogleLogin = oauthLogins.Contains("google");
            
            return new LoginMethods
            {
                DDDEA = hasDddeaLogin,
                GitHub = hasGitHubLogin,
                Twitter = hasTwitterLogin,
                Google = hasGoogleLogin
            };
        }
    }
}
