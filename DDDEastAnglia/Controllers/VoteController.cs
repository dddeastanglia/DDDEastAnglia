using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class VoteController : Controller
    {
        private readonly VotingCookieRepository _votingCookieRepository;

        public VoteController() : this(new VotingCookieRepository())
        {
            
        }

        public VoteController(VotingCookieRepository votingCookieRepository)
        {
            _votingCookieRepository = votingCookieRepository;
        }

        public ActionResult RegisterVote(int id)
        {
            var cookie = _votingCookieRepository.Get(Request, VotingCookie.CookieName);
            cookie.Add(id);
            _votingCookieRepository.Save(Response, cookie);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult RemoveVote(int id)
        {
            var cookie = _votingCookieRepository.Get(Request, VotingCookie.CookieName);
            cookie.Remove(id);
            _votingCookieRepository.Save(Response, cookie);
            return RedirectToAction("Index", "Session");
        }
    }
}