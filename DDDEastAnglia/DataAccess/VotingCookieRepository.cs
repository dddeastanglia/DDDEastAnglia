using System;
using System.Linq;
using System.Web;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IVotingCookieRepository : IRepository<VotingCookie, string>
    {
    }

    public class VotingCookieRepository : IVotingCookieRepository
    {
        private const string SessionListKey = "SessionVotes";
        private const string IdKey = "Id";

        public VotingCookie Get(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[VotingCookie.CookieName];
            if (cookie == null)
            {
                return new VotingCookie(cookieName);
            }
            Guid cookieId;
            if (string.IsNullOrWhiteSpace(cookie[IdKey]) || !Guid.TryParse(cookie[IdKey], out cookieId))
            {
                cookieId = Guid.NewGuid();
            }
            var sessionList = cookie[SessionListKey];
            if (string.IsNullOrWhiteSpace(sessionList))
            {
                return new VotingCookie(cookieId, cookieName, VotingCookie.DefaultExpiry);
            }
            var sessionIds = sessionList
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(TryParse)
                                .Where(val => val.HasValue)
                                .Select(val => val.Value);
            return new VotingCookie(cookieId, cookieName, sessionIds, VotingCookie.DefaultExpiry);
        }

        public void Update(VotingCookie entity)
        {
            Save(entity);
        }

        public void Save(VotingCookie cookie)
        {
            if (!HttpContext.Current.Request.Browser.Cookies)
            {
                return;
            }
            var listOfSessions = string.Join(",", cookie.SessionsVotedFor.ToArray());
            var httpCookie = new HttpCookie(cookie.Name);
            httpCookie.Expires = cookie.Expires;
            httpCookie[IdKey] = cookie.Id.ToString();
            httpCookie[SessionListKey] = listOfSessions;
            HttpContext.Current.Response.SetCookie(httpCookie);
        }

        public void Delete(VotingCookie entity)
        {
            if (!Exists(entity.Name))
            {
                return;
            }
            Save(new VotingCookie(entity.Id, VotingCookie.CookieName, DateTime.Now.AddDays(-1)));
        }

        public void Delete(string identifier)
        {
            Delete(Get(identifier));
        }

        public bool Exists(string identifier)
        {
            return HttpContext.Current.Request.Cookies[identifier] != null;
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