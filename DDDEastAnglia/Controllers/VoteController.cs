using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Context;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class VoteController : Controller
    {
        private readonly IVotingCookieRepository _votingCookieRepository;
        private readonly IVoteRepository _voteRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ITimeProvider _timeProvider;
        private readonly IRequestInformationProvider _requestInformationProvider;

        public VoteController() 
            : this(new VotingCookieRepository(), 
                   new EntityFrameworkVoteRepository(), 
                   new EntityFrameworkSessionRepository(), 
                   new EventRepository(), 
                   new TimeProvider(),
                   new HttpContextRequestInformationProvider())
        {
            
        }

        public VoteController(IVotingCookieRepository votingCookieRepository, 
            IVoteRepository voteRepository, 
            ISessionRepository sessionRepository, 
            IEventRepository eventRepository, 
            ITimeProvider timeProvider,
            IRequestInformationProvider requestInformationProvider)
        {
            _votingCookieRepository = votingCookieRepository;
            _voteRepository = voteRepository;
            _sessionRepository = sessionRepository;
            _eventRepository = eventRepository;
            _timeProvider = timeProvider;
            _requestInformationProvider = requestInformationProvider;
        }

        public ActionResult Status(int sessionId)
        {
            if (!_sessionRepository.Exists(sessionId))
            {
                return PartialView(new SessionVoteModel {SessionId = sessionId, UserCanVote = false, VotedForByUser = false});
            }
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            var currentEvent = _eventRepository.Get("DDDEA2013");
            var result = new SessionVoteModel();
            result.SessionId = sessionId;
            result.UserCanVote = currentEvent != null && currentEvent.CanVote();
            result.VotedForByUser = cookie.Contains(sessionId);
            return PartialView(result);
        }

        public ActionResult RegisterVote(int id)
        {
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            if (!_sessionRepository.Exists(id))
            {
                return RedirectToAction("Index", "Session");
            }
            if (cookie.Contains(id))
            {
                return RedirectToAction("Index", "Session");
            }
            cookie.Add(id);
            var vote = new Vote
                {
                    Event = "DDDEA2013",
                    IsVote = true,
                    SessionId = id,
                    CookieId = cookie.Id,
                    TimeRecorded = _timeProvider.UtcNow,
                    IPAddress = _requestInformationProvider.GetIPAddress(),
                    UserAgent = _requestInformationProvider.UserAgent,
                    Referer = _requestInformationProvider.Referrer
                };
            if (_requestInformationProvider.IsLoggedIn())
            {
                vote.UserId = _requestInformationProvider.GetCurrentUser().UserId;
            }
            _voteRepository.Save(vote);
            _votingCookieRepository.Save(cookie);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult RemoveVote(int id)
        {
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            if (!cookie.Contains(id))
            {
                return RedirectToAction("Index", "Session");
            }
            cookie.Remove(id);
            var vote = new Vote
                {
                    Event = "DDDEA2013",
                    IsVote = false,
                    SessionId = id,
                    CookieId = cookie.Id,
                    TimeRecorded = _timeProvider.UtcNow,
                    IPAddress = _requestInformationProvider.GetIPAddress(),
                    UserAgent = _requestInformationProvider.UserAgent,
                    Referer = _requestInformationProvider.Referrer
                };
            if (_requestInformationProvider.IsLoggedIn())
            {
                vote.UserId = _requestInformationProvider.GetCurrentUser().UserId;
            }
            _voteRepository.Save(vote);
            _votingCookieRepository.Save(cookie);
            return RedirectToAction("Index", "Session");
        }
    }
}