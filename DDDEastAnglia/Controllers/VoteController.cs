using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Commands.Vote;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Mvc.Attributes;

namespace DDDEastAnglia.Controllers
{
    public partial class VoteController : Controller, IRequestProvider
    {
        private readonly ISessionVoteModelProvider _sessionVoteModelProvider;
        private readonly IMessageBus _messageBus;
        private readonly IControllerInformationProvider _controllerInformationProvider; 

        public VoteController() 
            : this(Factory.GetSessionVoteModelProvider(),
                   Factory.GetMessageBus(),
                   Factory.GetControllerInformationProvider())
        {
            
        }

        public VoteController(ISessionVoteModelProvider sessionVoteModelProvider,
            IMessageBus messageBus,
            IControllerInformationProvider informationProvider)
        {
            _sessionVoteModelProvider = sessionVoteModelProvider;
            _messageBus = messageBus;
            _controllerInformationProvider = informationProvider;
        }

        public virtual ActionResult Status(int id)
        {
            var cookie = _controllerInformationProvider.GetCookie(VotingCookie.CookieName);
            var result = _sessionVoteModelProvider.Get(id, GetCookieId(cookie.Value));
            _controllerInformationProvider.SaveCookie(_controllerInformationProvider.GetCookie(VotingCookie.CookieName));
            _controllerInformationProvider.SaveCookie(cookie);
            return result.CanVote ? PartialView(result) as ActionResult : new EmptyResult();
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public virtual ActionResult RegisterVote(int id, VoteModel sessionVoteModel = null)
        {
            var cookie = _controllerInformationProvider.GetCookie(VotingCookie.CookieName);

            var width = sessionVoteModel != null ? sessionVoteModel.Width : 0;
            var height = sessionVoteModel != null ? sessionVoteModel.Height : 0;
            
            var vote = new RegisterVoteCommand
                        {
                            SessionId = id,
                            CookieId = GetCookieId(cookie.Value),
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
            _controllerInformationProvider.SaveCookie(cookie);
            return RedirectOrReturnPartialView(id);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public virtual ActionResult RemoveVote(int id, VoteModel sessionVoteModel = null)
        {
            var cookie = _controllerInformationProvider.GetCookie(VotingCookie.CookieName);
            var cookieId = GetCookieId(cookie.Value);
            _messageBus.Send(new DeleteVoteCommand
                {
                    SessionId = id,
                    CookieId = cookieId
                });
            _controllerInformationProvider.SaveCookie(cookie);
            return RedirectOrReturnPartialView(id);
        }

        private Guid GetCookieId(string value)
        {
            Guid guid;
            if (Guid.TryParse(value, out guid))
            {
                return guid;
            }
            return Guid.Empty;
        }

        private ActionResult RedirectOrReturnPartialView(int sessionId)
        {
            return _controllerInformationProvider.IsAjaxRequest
                       ? RedirectToAction("Status", new { id = sessionId})
                       : RedirectToAction("Index", "Session");
        }
    }
}