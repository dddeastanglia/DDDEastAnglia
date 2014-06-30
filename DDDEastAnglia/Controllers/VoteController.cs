using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Mvc.Attributes;

namespace DDDEastAnglia.Controllers
{
    public class VoteController : Controller
    {
        private readonly ISessionVoteModelQuery _sessionVoteModelQuery;
        private readonly IMessageBus _messageBus;
        private readonly IControllerInformationProvider _controllerInformationProvider; 

        public VoteController(ISessionVoteModelQuery sessionVoteModelQuery,
                                IMessageBus messageBus, IControllerInformationProvider informationProvider)
        {
            _sessionVoteModelQuery = sessionVoteModelQuery;
            _messageBus = messageBus;
            _controllerInformationProvider = informationProvider;
        }

        public ActionResult Status(int id)
        {
            var cookie = _controllerInformationProvider.GetVotingCookie();
            var result = _sessionVoteModelQuery.Get(id, cookie.Id);
            _controllerInformationProvider.SaveVotingCookie(cookie);
            return result.CanVote ? PartialView(result) as ActionResult : new EmptyResult();
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RegisterVote(int id, VoteModel sessionVoteModel = null)
        {
            var cookie = _controllerInformationProvider.GetVotingCookie();

            var width = sessionVoteModel != null ? sessionVoteModel.Width : 0;
            var height = sessionVoteModel != null ? sessionVoteModel.Height : 0;
            
            var vote = new RegisterVoteCommand
                        {
                            SessionId = id,
                            CookieId = cookie.Id,
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
            _messageBus.Send(vote);
            _controllerInformationProvider.SaveVotingCookie(cookie);
            return RedirectOrReturnPartialView(id);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RemoveVote(int id, VoteModel sessionVoteModel = null)
        {
            var cookie = _controllerInformationProvider.GetVotingCookie();
            var cookieId = cookie.Id;
            _messageBus.Send(new DeleteVoteCommand
                {
                    SessionId = id,
                    CookieId = cookieId
                });
            _controllerInformationProvider.SaveVotingCookie(cookie);
            return RedirectOrReturnPartialView(id);
        }

        private ActionResult RedirectOrReturnPartialView(int sessionId)
        {
            return _controllerInformationProvider.IsAjaxRequest
                       ? RedirectToAction("Status", "Vote", new { id = sessionId})
                       : RedirectToAction("Index", "Session");
        }
    }
}
