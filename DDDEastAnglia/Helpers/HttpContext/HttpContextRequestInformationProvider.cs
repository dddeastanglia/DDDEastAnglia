using System.Net;
using System.Text.RegularExpressions;

namespace DDDEastAnglia.Helpers.HttpContext
{
    public class HttpContextRequestInformationProvider : IRequestInformationProvider
    {
        private static readonly Regex IPV4AddressMatch = new Regex(@"\b(?<IPAddress>((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\b", RegexOptions.Compiled);
        public string GetIPAddress()
        {
            var request = System.Web.HttpContext.Current.Request;
            return MatchIPAddress(request.Headers["HTTP_X_FORWARDED_FOR"]) 
                   ?? MatchIPAddress(request.Headers["HTTP_VIA"])
                   ?? MatchIPAddress(request.Headers["HTTP_PROXY_CONNECTION"])
                   ?? MatchIPAddress(request.UserHostAddress);
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