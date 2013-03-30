using System;
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
        private const string DefaultEventName = "DDDEA2013";
        private DDDEAContext db = new DDDEAContext();
        private EventRepository eventRepository = new EventRepository();
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

            return View(new SessionIndexModel
                {
                    Sessions = displaySessions, 
                    IsOpenForSubmission = eventRepository.Get(DefaultEventName).CanSubmit(),
                });
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
                    SpeakerUserName = session.SpeakerUserName,
                    TweetLink = GetTweetLink(session.Title, Request.Url.ToString())
                };

            var userProfile = db.UserProfiles.First(s => s.UserName == session.SpeakerUserName);
            sessionDisplay.SpeakerName = userProfile.Name;
            sessionDisplay.SpeakerGravitarUrl = userProfile.GravitarUrl();
            return View(sessionDisplay);
        }

        //
        // GET: /Session/Create

        public virtual ActionResult Create()
        {
            if (!eventRepository.Get(DefaultEventName).CanSubmit())
            {
                return RedirectToAction("Index");
            }
            if (User == null || User.Identity == null)
            {
                return RedirectToAction("Index");
            }
            var userProfile = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (userProfile == null)
            {
                return RedirectToAction("Index");
            }
            return View(new Session { SpeakerUserName = userProfile.UserName });
        }

        //
        // POST: /Session/Create

        [HttpPost]
        public virtual ActionResult Create([Bind(Exclude = "Votes")]Session session)
        {
            if (!eventRepository.Get(DefaultEventName).CanSubmit())
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var addedSession = db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = addedSession.SessionId });
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

        private string GetTweetLink(string title, string sessionDetailsUrl)
        {
            var encodedUrl = Url.Encode(sessionDetailsUrl);
            var text = Url.Encode(string.Format("Posted a session for #dddea - {0} - {1} ", title, sessionDetailsUrl));
            return string.Format("https://twitter.com/intent/tweet?original_referer={0};text={1}", encodedUrl, text);
        }
    }

    
}