using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Context
{
    public class HttpContextRequestInformationProvider : IRequestInformationProvider
    {
        private DDDEAContext context = new DDDEAContext();
        private static readonly Regex IPV4AddressMatch = new Regex(@"\b(?<IPAddress>((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\b", RegexOptions.Compiled);

        public string UserAgent { get { return HttpContext.Current.Request.UserAgent; } }
        public string Referrer 
        { 
            get
            {
                return HttpContext.Current.Request.UrlReferrer == null ? null : HttpContext.Current.Request.UrlReferrer.ToString();
            }
        }

        public string SessionId
        {
            get { return HttpContext.Current.Session.SessionID; }
        }

        public string GetIPAddress()
        {
            var request = HttpContext.Current.Request;
            return MatchIPAddress(request.Headers["HTTP_X_FORWARDED_FOR"]) 
                   ?? MatchIPAddress(request.Headers["HTTP_VIA"])
                   ?? MatchIPAddress(request.Headers["HTTP_PROXY_CONNECTION"])
                   ?? MatchIPAddress(request.UserHostAddress);
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name);
        }

        public UserProfile GetCurrentUser()
        {
            return context.UserProfiles.FirstOrDefault(profile => profile.UserName == HttpContext.Current.User.Identity.Name);
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