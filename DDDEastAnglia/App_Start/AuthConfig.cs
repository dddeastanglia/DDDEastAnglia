using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;

using Microsoft.Web.WebPages.OAuth;
using DDDEastAnglia.Models;

namespace DDDEastAnglia
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(
                consumerKey: WebConfigurationManager.AppSettings["TwitterKey"],
                consumerSecret: WebConfigurationManager.AppSettings["TwitterSecret"]);

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            OAuthWebSecurity.RegisterGoogleClient();

            OAuthWebSecurity.RegisterClient(new GitHubOAuthClient(
                WebConfigurationManager.AppSettings["GitHubAppId"],
                WebConfigurationManager.AppSettings["GitHubSecret"]), "GitHub", null);
        }
    }
}
