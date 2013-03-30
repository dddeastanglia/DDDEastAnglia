using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public partial class SessionController : Controller
    {
        private DDDEAContext db = new DDDEAContext();

        //
        // GET: /Session/

        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            List<SessionDisplayModel> displaySessions = new List<SessionDisplayModel>();

            List<Session> sessions = db.Sessions.ToList();

            foreach (Session session in sessions)
            {
                SessionDisplayModel displaySession = new SessionDisplayModel()
                    {
                        SessionId = session.SessionId,
                        SessionTitle = session.Title,
                        SpeakerUserName = session.SpeakerUserName,
                        SessionAbstract = session.Abstract
                    };
                    displaySession.SpeakerName = db.UserProfiles.First(s => s.UserName == session.SpeakerUserName).Name;
                displaySessions.Add(displaySession);
            }

            return View(displaySessions);
        }

        //
        // GET: /Session/Details/5

        [AllowAnonymous]
        public virtual ActionResult Details(int id = 0)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            SessionDisplayModel sessionDisplay = new SessionDisplayModel()
                {
                    SessionAbstract = session.Abstract,
                    SessionId = session.SessionId,
                    SessionTitle = session.Title,
                    SpeakerUserName = session.SpeakerUserName
                };

            sessionDisplay.SpeakerName = db.UserProfiles.First(s => s.UserName == session.SpeakerUserName).Name;
      
            return View(sessionDisplay);
        }

        //
        // GET: /Session/Create

        public virtual ActionResult Create()
        {
            return View(new Session { SpeakerUserName = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name).UserName });
        }

        //
        // POST: /Session/Create

        [HttpPost]
        public virtual ActionResult Create([Bind(Exclude = "Votes")]Session session)
        {
            if (ModelState.IsValid)
            {
                db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        //
        // GET: /Session/Edit/5

        public virtual ActionResult Edit(int id = 0)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        //
        // POST: /Session/Edit/5

        [HttpPost]
        public virtual ActionResult Edit([Bind(Exclude = "Votes")]Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(session);
        }

        //
        // GET: /Session/Delete/5

        public virtual ActionResult Delete(int id = 0)
        {
            Session session = db.Sessions.Find(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        //
        // POST: /Session/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}