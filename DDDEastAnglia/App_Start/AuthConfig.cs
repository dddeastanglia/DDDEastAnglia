﻿using System.Web.Configuration;
using Microsoft.Web.WebPages.OAuth;

namespace DDDEastAnglia
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            // NOTE: if you add/edit this list, be sure to edit the OAuth provider links in 
            //          the menu and the registration page as they don't get picked up automatically

            OAuthWebSecurity.RegisterClient(new GitHubOAuthClient(
                WebConfigurationManager.AppSettings["GitHubAppId"],
                WebConfigurationManager.AppSettings["GitHubSecret"]), "GitHub", null);
            
            OAuthWebSecurity.RegisterTwitterClient(
                WebConfigurationManager.AppSettings["TwitterKey"],
                WebConfigurationManager.AppSettings["TwitterSecret"]);

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
