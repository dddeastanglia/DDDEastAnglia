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
        public VotingCookie Get(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[VotingCookie.CookieName];
            if (cookie == null)
            {
                return new VotingCookie(cookieName);
            }
            Guid cookieId;
            if (!string.IsNullOrWhiteSpace(cookie["Id"]) || !Guid.TryParse(cookie["Id"], out cookieId))
            {
                cookieId = Guid.NewGuid();
            }
            var sessionList = cookie.Value;
            if (string.IsNullOrWhiteSpace(sessionList))
            {
                return new VotingCookie(cookieId, cookieName, cookie.Expires);
            }
            var sessionIds = sessionList
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(TryParse)
                                .Where(val => val.HasValue)
                                .Select(val => val.Value);
            return new VotingCookie(cookieId, cookieName, sessionIds, cookie.Expires);
        }

        public void Update(VotingCookie entity)
        {
            Save(entity);
        }

        public void Save(VotingCookie cookie)
        {
            var listOfSessions = string.Join(",", cookie.SessionsVotedFor.ToArray());
            var httpCookie = new HttpCookie(cookie.Name, listOfSessions);
            httpCookie.Expires = cookie.Expires;
            httpCookie["Id"] = cookie.Id.ToString();
            var response = HttpContext.Current.Response;
            if (response.Cookies[httpCookie.Name] != null)
            {
                response.Cookies.Set(httpCookie);
            }
            else
            {
                response.Cookies.Add(httpCookie);
            }
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