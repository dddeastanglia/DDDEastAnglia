using System;
using System.Linq;
using System.Web;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IVotingCookieRepository
    {
        VotingCookie Get(HttpRequestBase request, string cookieName);
        void Save(HttpResponseBase response, VotingCookie cookie);
    }

    public class VotingCookieRepository : IVotingCookieRepository
    {
        public VotingCookie Get(HttpRequestBase request, string cookieName)
        {
            var cookie = request.Cookies[VotingCookie.CookieName];
            if (cookie == null)
            {
                return new VotingCookie(cookieName);
            }
            var sessionList = cookie.Value;
            if (string.IsNullOrWhiteSpace(sessionList))
            {
                return new VotingCookie(cookieName, cookie.Expires);
            }
            var sessionIds = sessionList
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(TryParse)
                                .Where(val => val.HasValue)
                                .Select(val => val.Value);
            return new VotingCookie(cookieName, sessionIds, cookie.Expires);
        }

        public void Save(HttpResponseBase response, VotingCookie cookie)
        {
            var listOfSessions = string.Join(",", cookie.SessionsVotedFor.ToArray());
            var httpCookie = new HttpCookie(cookie.Name, listOfSessions);
            httpCookie.Expires = cookie.Expires;
            if (response.Cookies[httpCookie.Name] != null)
            {
                response.Cookies.Set(httpCookie);
            }
            else
            {
                response.Cookies.Add(httpCookie);
            }
        }

        private static int? TryParse(string value)
        {
            int val;
            if (int.TryParse(value, out val))
            {
                return val;
            }
            return null;
        }
    }
}