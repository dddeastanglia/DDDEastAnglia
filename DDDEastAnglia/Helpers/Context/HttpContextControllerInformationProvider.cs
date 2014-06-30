using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Context
{
    public class HttpContextControllerInformationProvider : IControllerInformationProvider
    {
        private readonly IVotingCookieFactory votingCookieFactory;
        private readonly IUserProfileRepository userProfileRepository;

        public HttpContextControllerInformationProvider(IVotingCookieFactory votingCookieFactory, IUserProfileRepository userProfileRepository)
        {
            if (votingCookieFactory == null)
            {
                throw new ArgumentNullException("votingCookieFactory");
            }
            
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            this.votingCookieFactory = votingCookieFactory;
            this.userProfileRepository = userProfileRepository;
        }

        private static readonly Regex IPV4AddressMatch = new Regex(@"\b(?<IPAddress>((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\b", RegexOptions.Compiled);

        public string UserAgent { get { return HttpContext.Current.Request.UserAgent; } }

        public string Referrer 
        { 
            get
            {
                return HttpContext.Current.Request.UrlReferrer == null ? null : HttpContext.Current.Request.UrlReferrer.ToString();
            }
        }

        public string SessionId { get { return HttpContext.Current.Session.SessionID; } }

        public bool IsAjaxRequest { get { return new HttpRequestWrapper(HttpContext.Current.Request).IsAjaxRequest(); } }

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
            return userProfileRepository.GetAllUserProfiles().FirstOrDefault(profile => profile.UserName == HttpContext.Current.User.Identity.Name);
        }

        public DateTime UtcNow { get { return DateTime.UtcNow; } }

        public HttpCookie GetCookie()
        {
            var votingCookie = votingCookieFactory.Create();
            var httpCookie = HttpContext.Current.Request.Cookies[votingCookie.Name]
                                ?? new HttpCookie(votingCookie.Name, Guid.NewGuid().ToString());
            httpCookie.Expires = votingCookie.Expiry;
            return httpCookie;
        }

        public void SaveCookie(HttpCookie httpCookie)
        {
            HttpContext.Current.Response.SetCookie(httpCookie);
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