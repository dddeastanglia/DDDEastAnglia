using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Models
{
    public class VotingCookie
    {
        public const string CookieName = "DDDEA2013.Voting";
        private static readonly DateTime DefaultExpiry = new DateTime(2013, 4, 30);
        private readonly List<int> _sessionsVotedFor = new List<int>();
        private DateTime _expires;

        public VotingCookie()
            : this(DefaultExpiry)
        {
            
        }

        public VotingCookie(DateTime expires)
        {
            _expires = expires;
        }

        public VotingCookie(IEnumerable<int> sessionsVotedFor, DateTime expires) 
            : this(expires)
        {
            _sessionsVotedFor.AddRange(sessionsVotedFor);
        }

        public IEnumerable<int> SessionsVotedFor { get { return _sessionsVotedFor.AsReadOnly(); } }


        public bool Contains(int sessionId)
        {
            return _sessionsVotedFor.Contains(sessionId);
        }

        public void Add(int sessionId)
        {
            if (_sessionsVotedFor.Contains(sessionId))
            {
                return;
            }
            _sessionsVotedFor.Add(sessionId);
        }

        public void Remove(int sessionId)
        {
            if (!_sessionsVotedFor.Contains(sessionId))
            {
                return;
            }
            _sessionsVotedFor.Remove(sessionId);
        }

        public void Save(HttpResponseBase response)
        {
            var listOfSessions = string.Join(",", _sessionsVotedFor.ToArray());
            var cookie = new HttpCookie(CookieName, listOfSessions);
            cookie.Expires = _expires;
            if (response.Cookies[CookieName] != null)
            {
                response.Cookies.Set(cookie);
            }
            else
            {
                response.Cookies.Add(cookie);
            }
        }

        public static VotingCookie Get(HttpRequestBase request)
        {
            var cookie = request.Cookies[CookieName];
            if (cookie == null)
            {
                return new VotingCookie();
            }
            var sessionList = cookie.Value;
            if (string.IsNullOrWhiteSpace(sessionList))
            {
                return new VotingCookie(cookie.Expires);
            }
            var sessionIds = sessionList
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(TryParse)
                                .Where(val => val.HasValue)
                                .Select(val => val.Value);
            return new VotingCookie(sessionIds, cookie.Expires);
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