using System;
using System.Linq;
using System.Web;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers.Context
{
    public class HttpCookieVoteRepository : ICurrentUserVoteRepository
    {
        private const string SessionListKey = "SessionVotes";
        private const string IdKey = "Id";
        private readonly IVoteRepository _repository;

        public HttpCookieVoteRepository(IVoteRepository repository)
        {
            _repository = repository;
        }

        public bool HasVotedFor(int sessionId)
        {
            return Get().SessionsVotedFor.Contains(sessionId);
        }

        public void Save(Vote vote)
        {
            _repository.Save(vote);
            var votingCookie = Get();

            votingCookie.Add(vote.SessionId);

            var newCookie = new HttpCookie(votingCookie.Name);
            newCookie.Expires = votingCookie.Expires;
            newCookie[IdKey] = votingCookie.Id.ToString();
            newCookie[SessionListKey] = string.Join(",", votingCookie.SessionsVotedFor);
            HttpContext.Current.Response.SetCookie(newCookie);
        }

        public void Delete(int sessionId)
        {
            var votingCookie = Get();
            _repository.Delete(sessionId, votingCookie.Id);

            votingCookie.Remove(sessionId);

            var newCookie = new HttpCookie(votingCookie.Name);
            newCookie.Expires = votingCookie.Expires;
            newCookie[IdKey] = votingCookie.Id.ToString();
            newCookie[SessionListKey] = string.Join(",", votingCookie.SessionsVotedFor);
            HttpContext.Current.Response.SetCookie(newCookie);
        }

        private static VotingCookie Get()
        {
            var cookie = HttpContext.Current.Request.Cookies[VotingCookie.CookieName];
            Guid cookieId;
            if (cookie == null || string.IsNullOrWhiteSpace(cookie[IdKey]) || !Guid.TryParse(cookie[IdKey], out cookieId))
            {
                cookieId = Guid.NewGuid();
            }
            var sessionList = cookie != null ? cookie[SessionListKey] : "";
            var sessionIds = sessionList
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(TryParse)
                                .Where(val => val.HasValue)
                                .Select(val => val.Value)
                                .ToList();
            return new VotingCookie(cookieId, sessionIds);
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