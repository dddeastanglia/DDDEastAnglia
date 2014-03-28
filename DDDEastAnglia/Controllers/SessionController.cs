using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private const string DefaultEventName = "DDDEA2013";
        private readonly DDDEAContext db = new DDDEAContext();
        private readonly IConferenceRepository _conferenceRepository;
        private readonly ISessionSorter _sessionSorter;


        public SessionController(IConferenceRepository conferenceRepository, ISessionSorter sorter)
        {
            _conferenceRepository = conferenceRepository;
            _sessionSorter = sorter;
        }

        // GET: /Session/
        [AllowAnonymous]
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            var speakersLookup = db.UserProfiles.ToDictionary(p => p.UserName, p => p);
            var sessions = db.Sessions;

            var allSessions = new List<SessionDisplayModel>();

            foreach (var session in sessions)
            {
                var profile = speakersLookup[session.SpeakerUserName];
                var displayModel = CreateDisplayModel(session, profile);
                allSessions.Add(displayModel);
            }

            _sessionSorter.SortSessions(_conferenceRepository.GetByEventShortName(DefaultEventName), allSessions);

            return View(new SessionIndexModel
                        {
                            Sessions = allSessions,
                            IsOpenForSubmission = _conferenceRepository.GetByEventShortName(DefaultEventName).CanSubmit(),
                            IsOpenForVoting = _conferenceRepository.GetByEventShortName(DefaultEventName).CanVote()
                        });
        }

        // GET: /Session/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Session session = db.Sessions.Find(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            var userProfile = db.UserProfiles.First(s => s.UserName == session.SpeakerUserName);
            var displayModel = CreateDisplayModel(session, userProfile);
            displayModel.SpeakerGravatarUrl = userProfile.GravatarUrl();
            return View(displayModel);
        }

        // GET: /Session/Create
        public ActionResult Create()
        {
            var conference = _conferenceRepository.GetByEventShortName(DefaultEventName);
            if (!conference.CanSubmit())
            {
                return RedirectToAction("Index");
            }

            if (User == null)
            {
                return RedirectToAction("Index");
            }

            var userProfile = db.UserProfiles.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (userProfile == null)
            {
                return RedirectToAction("Index");
            }

            return View(new Session {SpeakerUserName = userProfile.UserName, ConferenceId = conference.Id});
        }

        // POST: /Session/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Votes")] Session session)
        {
            if (!_conferenceRepository.GetByEventShortName(DefaultEventName).CanSubmit())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var addedSession = db.Sessions.Add(session);
                db.SaveChanges();
                return RedirectToAction("Details", new {id = addedSession.SessionId});
            }

            return View(session);
        }

        // GET: /Session/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Session session = db.Sessions.Find(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            return View(session);
        }

        // POST: /Session/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Votes")] Session session)
        {
            if (ModelState.IsValid)
            {
                db.Entry(session).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: /Session/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Session session = db.Sessions.Find(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            var userProfile = db.UserProfiles.First(s => s.UserName == session.SpeakerUserName);
            var displayModel = CreateDisplayModel(session, userProfile);
            return View(displayModel);
        }

        // POST: /Session/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Session session = db.Sessions.Find(id);
            db.Sessions.Remove(session);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private SessionDisplayModel CreateDisplayModel(Session session, UserProfile profile)
        {
            var isUsersSession = Request.IsAuthenticated && session.SpeakerUserName == User.Identity.Name;
            var tweetLink = CreateTweetLink(isUsersSession, session.Title,
                                            Url.Action("Details", "Session", new {id = session.SessionId},
                                                       Request.Url.Scheme));

            var displayModel = new SessionDisplayModel
                {
                    SessionId = session.SessionId,
                    SessionTitle = session.Title,
                    SessionAbstract = session.Abstract,
                    SpeakerId = profile.UserId,
                    SpeakerName = profile.Name,
                    SpeakerUserName = session.SpeakerUserName,
                    SpeakerGravatarUrl = profile.GravatarUrl(),
                    TweetLink = tweetLink,
                    IsUsersSession = isUsersSession
                };
            return displayModel;
        }

        private SessionTweetLink CreateTweetLink(bool isUsersSession, string sessionTitle, string sessionUrl)
        {
            var title = string.Format("Check out {0} session for #dddea - {1} {2}",
                                      isUsersSession ? "my" : "this",
                                      sessionTitle, sessionUrl);
            var tweetLink = new SessionTweetLink
                {
                    Title = title,
                    Url = sessionUrl
                };
            return tweetLink;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}