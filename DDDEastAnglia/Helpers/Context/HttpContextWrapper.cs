using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Context
{
    public class HttpContextWrapper : Request
    {
        private DDDEAContext context = new DDDEAContext();
        private static readonly Regex IPV4AddressMatch = new Regex(@"\b(?<IPAddress>((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\b", RegexOptions.Compiled);

        private readonly HttpContext _wrappedContext;

        public HttpContextWrapper(HttpContext wrappedContext)
        {
            _wrappedContext = wrappedContext;
        }

        public override string UserAgent
        {
            get { return _wrappedContext.Request.UserAgent; }
        }

        public override string Referrer
        {
            get { return _wrappedContext.Request.UrlReferrer != null ? _wrappedContext.Request.UrlReferrer.ToString() : null; }
        }

        public override string SessionId
        {
            get { return _wrappedContext.Session.SessionID; }
        }

        public override bool IsAjaxRequest
        {
            get { return new HttpRequestWrapper(_wrappedContext.Request).IsAjaxRequest(); }
        }

        public override string GetIPAddress()
        {
            var request = HttpContext.Current.Request;
            return MatchIPAddress(request.Headers["HTTP_X_FORWARDED_FOR"])
                   ?? MatchIPAddress(request.Headers["HTTP_VIA"])
                   ?? MatchIPAddress(request.Headers["HTTP_PROXY_CONNECTION"])
                   ?? MatchIPAddress(request.UserHostAddress);
        }

        public override bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name);
        }

        public override UserProfile GetCurrentUser()
        {
            return context.UserProfiles.FirstOrDefault(profile => profile.UserName == HttpContext.Current.User.Identity.Name);
        }

        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }

        public static string MatchIPAddress(string potentialIPAddress)
        {
            if (string.IsNullOrEmpty(potentialIPAddress))
            {
                return null;
            }
            var match = IPV4AddressMatch.Match(potentialIPAddress);
            if (match.Success)
            {
                potentialIPAddress = match.Groups["IPAddress"].Value;
            }
            IPAddress ipAddress;
            if (IPAddress.TryParse(potentialIPAddress, out ipAddress))
            {
                return ipAddress.ToString();
            }
            return null;
        }
    }
}