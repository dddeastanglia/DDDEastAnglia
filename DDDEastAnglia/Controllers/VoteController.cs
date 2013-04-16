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
        private readonly ICurrentUserVoteRepository _currentUserVoteRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IControllerInformationProvider _controllerInformationProvider;

        public VoteController() 
            : this(new HttpCookieVoteRepository(new EntityFrameworkVoteRepository()), 
                   new EntityFrameworkSessionRepository(), 
                   new EventRepository(), 
                   new HttpContextControllerInformationProvider())
        {
            
        }

        public VoteController(ICurrentUserVoteRepository currentUserVoteRepository, 
            ISessionRepository sessionRepository, 
            IEventRepository eventRepository, 
            IControllerInformationProvider controllerInformationProvider)
        {
            _currentUserVoteRepository = currentUserVoteRepository;
            _sessionRepository = sessionRepository;
            _eventRepository = eventRepository;
            _controllerInformationProvider = controllerInformationProvider;
        }

        public ActionResult Status(int id)
        {
            if (!_sessionRepository.Exists(id))
            {
                return new EmptyResult();
            }
            var currentEvent = _eventRepository.Get("DDDEA2013");
            var result = new SessionVoteModel();
            result.SessionId = id;
            result.VotedForByUser = _currentUserVoteRepository.HasVotedFor(id);
            return currentEvent.CanVote() ? PartialView(result) as ActionResult : new EmptyResult();
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RegisterVote(int id, VoteModel voteModel = null)
        {
            var currentEvent = _eventRepository.Get("DDDEA2013");
            if (!_sessionRepository.Exists(id))
            {
                return RedirectOrReturnPartialView(id);
            }
            if (_currentUserVoteRepository.HasVotedFor(id))
            {
                return RedirectOrReturnPartialView(id);
            }
            if (currentEvent == null || !currentEvent.CanVote())
            {
                return RedirectOrReturnPartialView(id);
            }
            var width = voteModel != null ? voteModel.Width : 0;
            var height = voteModel != null ? voteModel.Height : 0;
            var vote = new Vote
                        {
                            Event = "DDDEA2013",
                            SessionId = id,
                            TimeRecorded = _controllerInformationProvider.UtcNow,
                            IPAddress = _controllerInformationProvider.GetIPAddress(),
                            UserAgent = _controllerInformationProvider.UserAgent,
                            Referrer = _controllerInformationProvider.Referrer,
                            WebSessionId = _controllerInformationProvider.SessionId
                        };
            if (_controllerInformationProvider.IsLoggedIn())
            {
                vote.UserId = _controllerInformationProvider.GetCurrentUser().UserId;
            }
            if (width != 0 || height != 0)
            {
                vote.ScreenResolution = string.Format("{0}x{1}", width, height);
            }
            _currentUserVoteRepository.Save(vote);
            return RedirectOrReturnPartialView(id);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RemoveVote(int id, VoteModel voteModel = null)
        {
            var currentEvent = _eventRepository.Get("DDDEA2013");
            if (!_currentUserVoteRepository.HasVotedFor(id))
            {
                return RedirectOrReturnPartialView(id);
            }
            if (currentEvent == null || !currentEvent.CanVote())
            {
                return RedirectOrReturnPartialView(id);
            }
            _currentUserVoteRepository.Delete(id);
            return RedirectOrReturnPartialView(id);
        }

        private ActionResult RedirectOrReturnPartialView(int sessionId)
        {
            return _controllerInformationProvider.IsAjaxRequest
                       ? RedirectToAction("Status", new { id = sessionId})
                       : RedirectToAction("Index", "Session");
        }
    }
}