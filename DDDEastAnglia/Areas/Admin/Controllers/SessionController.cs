using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SessionController : Controller
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IVoteRepository voteRepository;

        public SessionController(ISessionRepository sessionRepository, IVoteRepository voteRepository)
        {
            if (sessionRepository == null)
            {
                throw new ArgumentNullException(nameof(sessionRepository));
            }

            if (voteRepository == null)
            {
                throw new ArgumentNullException(nameof(voteRepository));
            }

            this.sessionRepository = sessionRepository;
            this.voteRepository = voteRepository;
        }

        public ActionResult Index()
        {
            var votesGroupedBySessionId = voteRepository.GetAllVotes().GroupBy(v => v.SessionId).ToDictionary(g => g.Key, g => g.Count());
            var sessions = sessionRepository.GetAllSessions().ToList();

            foreach (var session in sessions)
            {
                int voteCount;
                votesGroupedBySessionId.TryGetValue(session.SessionId, out voteCount);
                session.Votes = voteCount;
            }

            var orderedSessions = sessions.OrderByDescending(s => s.Votes).ToList();
            return View(orderedSessions);
        }

        public ActionResult Details(int id)
        {
            var session = sessionRepository.Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        public ActionResult Edit(int id)
        {
            var session = sessionRepository.Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Session session)
        {
            if (ModelState.IsValid)
            {
                sessionRepository.UpdateSession(session);
                return RedirectToAction("Index");
            }

            return View(session);
        }

        public ActionResult Delete(int id = 0)
        {
            var session = sessionRepository.Get(id);
            return session == null ? (ActionResult) HttpNotFound() : View(session);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sessionRepository.DeleteSession(id);
            return RedirectToAction("Index");
        }
    }
}
