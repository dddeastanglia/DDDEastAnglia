﻿using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Helpers;
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

        public VoteController() : this(new VotingCookieRepository(), new EntityFrameworkVoteRepository(), new EntityFrameworkSessionRepository(), new EventRepository(), new TimeProvider())
        {
            
        }

        public VoteController(IVotingCookieRepository votingCookieRepository, 
            IVoteRepository voteRepository, 
            ISessionRepository sessionRepository, 
            IEventRepository eventRepository, 
            ITimeProvider timeProvider)
        {
            _votingCookieRepository = votingCookieRepository;
            _voteRepository = voteRepository;
            _sessionRepository = sessionRepository;
            _eventRepository = eventRepository;
            _timeProvider = timeProvider;
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
            _voteRepository.Save(new Vote
                            {
                                Event = "DDDEA2013",
                                IsVote = true,
                                SessionId = id,
                                CookieId = cookie.Id, 
                                TimeRecorded = _timeProvider.UtcNow
                            });
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
            _voteRepository.Save(new Vote
                {
                    Event = "DDDEA2013",
                    IsVote = false,
                    SessionId = id,
                });
            _votingCookieRepository.Save(cookie);
            return RedirectToAction("Index", "Session");
        }
    }
}