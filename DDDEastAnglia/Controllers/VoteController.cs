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
        private readonly ISessionRepository _sessionRepository;

        public VoteController() : this(new VotingCookieRepository(), new EntityFrameworkVoteRepository(), new EntityFrameworkSessionRepository())
        {
            
        }

        public VoteController(IVotingCookieRepository votingCookieRepository, IVoteRepository voteRepository, ISessionRepository sessionRepository)
        {
            _votingCookieRepository = votingCookieRepository;
            _voteRepository = voteRepository;
            _sessionRepository = sessionRepository;
        }

        public ActionResult RegisterVote(int id)
        {
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            if (!_sessionRepository.Exists(id))
            {
                return RedirectToAction("Index", "Session");
            }
            cookie.Add(id);
            _voteRepository.Save(new Vote());
            _votingCookieRepository.Save(cookie);
            return RedirectToAction("Index", "Session");
        }

        public ActionResult RemoveVote(int id)
        {
            var cookie = _votingCookieRepository.Get(VotingCookie.CookieName);
            cookie.Remove(id);
            _votingCookieRepository.Save(cookie);
            return RedirectToAction("Index", "Session");
        }
    }
}