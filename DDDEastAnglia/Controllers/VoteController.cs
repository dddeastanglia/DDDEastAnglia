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
    public class VoteController : Controller
    {
        private readonly IMessageBus messageBus;
        private readonly ISessionVoteModelQuery sessionVoteModelQuery;
        private readonly IControllerInformationProvider controllerInformationProvider;

        public VoteController(IMessageBus messageBus, 
            ISessionVoteModelQuery sessionVoteModelQuery,
            IControllerInformationProvider controllerInformationProvider)
        {
            if (messageBus == null)
            {
                throw new ArgumentNullException("messageBus");
            }
            
            if (sessionVoteModelQuery == null)
            {
                throw new ArgumentNullException("sessionVoteModelQuery");
            }

            if (controllerInformationProvider == null)
            {
                throw new ArgumentNullException("controllerInformationProvider");
            }

            this.messageBus = messageBus;
            this.sessionVoteModelQuery = sessionVoteModelQuery;
            this.controllerInformationProvider = controllerInformationProvider;
        }

        public ActionResult Status(int id)
        {
            var cookie = controllerInformationProvider.GetCookie(VotingCookie.CookieName);
            var result = sessionVoteModelQuery.Get(id, GetCookieId(cookie.Value));
            controllerInformationProvider.SaveCookie(controllerInformationProvider.GetCookie(VotingCookie.CookieName));
            controllerInformationProvider.SaveCookie(cookie);
            return result.CanVote ? PartialView(result) as ActionResult : new EmptyResult();
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RegisterVote(int id, VoteModel sessionVoteModel = null)
        {
            var cookie = controllerInformationProvider.GetCookie(VotingCookie.CookieName);
            
            var vote = new RegisterVoteCommand
                        {
                            SessionId = id,
                            CookieId = GetCookieId(cookie.Value),
                            TimeRecorded = controllerInformationProvider.UtcNow,
                            IPAddress = controllerInformationProvider.GetIPAddress(),
                            UserAgent = controllerInformationProvider.UserAgent,
                            Referrer = controllerInformationProvider.Referrer,
                            WebSessionId = controllerInformationProvider.SessionId
                        };

            if (controllerInformationProvider.IsLoggedIn())
            {
                vote.UserId = controllerInformationProvider.GetCurrentUser().UserId;
            }

            if (sessionVoteModel != null)
            {
                if (sessionVoteModel.Width != 0 || sessionVoteModel.Height != 0)
                {
                    vote.ScreenResolution = string.Format("{0}x{1}", sessionVoteModel.Width, sessionVoteModel.Height);
                }

                vote.PositionInList = sessionVoteModel.PositionInList;
            }
            
            messageBus.Send(vote);
            controllerInformationProvider.SaveCookie(cookie);
            return RedirectOrReturnPartialView(id);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult RemoveVote(int id, VoteModel sessionVoteModel = null)
        {
            var cookie = controllerInformationProvider.GetCookie(VotingCookie.CookieName);
            var cookieId = GetCookieId(cookie.Value);
            messageBus.Send(new DeleteVoteCommand
                {
                    SessionId = id,
                    CookieId = cookieId
                });
            
            controllerInformationProvider.SaveCookie(cookie);
            return RedirectOrReturnPartialView(id);
        }

        private Guid GetCookieId(string value)
        {
            Guid guid;
            return Guid.TryParse(value, out guid) ? guid : Guid.Empty;
        }

        private ActionResult RedirectOrReturnPartialView(int sessionId)
        {
            return controllerInformationProvider.IsAjaxRequest
                       ? RedirectToAction("Status", "Vote", new { id = sessionId})
                       : RedirectToAction("Index", "Session");
        }
    }
}
