using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class VoteController : Controller
    {
        private readonly IVotingCookieRepository _votingCookieRepository;
        private readonly IVoteRepository _voteRepository;

        public VoteController() : this(new VotingCookieRepository(), new EntityFrameworkVoteRepository())
        {
            
        }

        public VoteController(IVotingCookieRepository votingCookieRepository, IVoteRepository voteRepository)
        {
            _votingCookieRepository = votingCookieRepository;
            _voteRepository = voteRepository;
        }

        public ActionResult RegisterVote(int id)
        {
            var cookie = _votingCookieRepository.Get(Request, VotingCookie.CookieName);
            cookie.Add(id);
            _voteRepository.Save(new Vote());
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