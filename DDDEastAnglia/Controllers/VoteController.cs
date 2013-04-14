using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Context;
using DDDEastAnglia.Models;
using DDDEastAnglia.Mvc.Attributes;

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

        public ActionResult Status(int id)
        {
            if (!_sessionRepository.Exists(id))
            {
                return PartialView(new SessionVoteModel {SessionId = id, UserCanVote = false, VotedForByUser = false});
            }
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            return GetVotePartialView(id, cookie);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RegisterVote(int id, VoteModel voteModel = null)
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
            var width = voteModel.Width;
            var height = voteModel.Height;
            cookie.Add(id);
            var vote = ProcessVote(id, cookie, width, height);
            _voteRepository.Save(vote);
            return RedirectOrReturnPartialView(id, cookie);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RemoveVote(int id, VoteModel voteModel = null)
        {
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            if (!cookie.Contains(id))
            {
                return RedirectToAction("Index", "Session");
            }
            cookie.Remove(id);
            _voteRepository.Delete(id, cookie.Id);
            _votingCookieRepository.Save(cookie);
            return RedirectOrReturnPartialView(id, cookie);
        }

        private ActionResult RedirectOrReturnPartialView(int sessionId, VotingCookie cookie)
        {
            return Request.IsAjaxRequest()
                       ? RedirectToAction("Status", new { id = sessionId})
                       : RedirectToAction("Index", "Session");
        }

        private Vote ProcessVote(int id, VotingCookie cookie, int width, int height)
        {
            var vote = new Vote
                {
                    Event = "DDDEA2013",
                    SessionId = id,
                    CookieId = cookie.Id,
                    TimeRecorded = _timeProvider.UtcNow,
                    IPAddress = _requestInformationProvider.GetIPAddress(),
                    UserAgent = _requestInformationProvider.UserAgent,
                    Referrer = _requestInformationProvider.Referrer,
                    WebSessionId = _requestInformationProvider.SessionId
                };
            if (_requestInformationProvider.IsLoggedIn())
            {
                vote.UserId = _requestInformationProvider.GetCurrentUser().UserId;
            }
            if (width != 0 || height != 0)
            {
                vote.ScreenResolution = string.Format("{0}x{1}", width, height);
            }
            _votingCookieRepository.Save(cookie);
            return vote;
        }

        private ActionResult GetVotePartialView(int sessionId, VotingCookie cookie)
        {
            var currentEvent = _eventRepository.Get("DDDEA2013");
            var result = new SessionVoteModel();
            result.SessionId = sessionId;
            result.UserCanVote = currentEvent != null && currentEvent.CanVote();
            result.VotedForByUser = cookie.Contains(sessionId);
            return PartialView(result);
        }
    }

    public class VoteModel
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string From { get; set; }
    }
}