using System.Web.Configuration;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            var githubAppId = WebConfigurationManager.AppSettings["GitHubAppId"];
            var githubSecret = WebConfigurationManager.AppSettings["GitHubSecret"];
            var gitHubOAuthClient = new GitHubOAuthClient(githubAppId, githubSecret);
            OAuthWebSecurity.RegisterClient(gitHubOAuthClient, "GitHub", null);

            var twitterKey = WebConfigurationManager.AppSettings["TwitterKey"];
            var twitterSecret = WebConfigurationManager.AppSettings["TwitterSecret"];
            OAuthWebSecurity.RegisterTwitterClient(twitterKey, twitterSecret);

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
