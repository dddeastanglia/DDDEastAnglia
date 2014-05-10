using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ISessionRepository sessionRepository;
        private readonly ISessionSorter sessionSorter;

        public SessionController(IConferenceLoader conferenceLoader, IUserProfileRepository userProfileRepository, ISessionRepository sessionRepository, ISessionSorter sorter)
        {
            this.conferenceLoader = conferenceLoader;
            this.userProfileRepository = userProfileRepository;
            this.sessionRepository = sessionRepository;
            sessionSorter = sorter;
        }

        [AllowAnonymous]
        [AllowCrossSiteJson]
        public ActionResult Index()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanShowSessions())
            {
                return HttpNotFound();
            }

            var speakersLookup = userProfileRepository.GetAllUserProfiles().ToDictionary(p => p.UserName, p => p);
            var sessions = sessionRepository.GetAllSessions();

            var allSessions = new List<SessionDisplayModel>();

            foreach (var session in sessions)
            {
                var profile = speakersLookup[session.SpeakerUserName];
                var displayModel = CreateDisplayModel(session, profile);
                allSessions.Add(displayModel);
            }

            sessionSorter.SortSessions(conference, allSessions);

            return View(new SessionIndexModel
                        {
                            Sessions = allSessions,
                            IsOpenForSubmission = conference.CanSubmit(),
                            IsOpenForVoting = conference.CanVote()
                        });
        }

        [AllowAnonymous]
        public ActionResult Details(int id = 0)
        {
            Session session = sessionRepository.Get(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            var userProfile = userProfileRepository.GetUserProfileByUserName(session.SpeakerUserName);
            var displayModel = CreateDisplayModel(session, userProfile);
            displayModel.SpeakerGravatarUrl = userProfile.GravatarUrl();
            return View(displayModel);
        }

        public ActionResult Create()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanSubmit())
            {
                return RedirectToAction("Index");
            }

            if (User == null)
            {
                return RedirectToAction("Index");
            }

            var userProfile = userProfileRepository.GetUserProfileByUserName(User.Identity.Name);

            if (userProfile == null)
            {
                return RedirectToAction("Index");
            }

            return View(new Session {SpeakerUserName = userProfile.UserName, ConferenceId = conference.Id});
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Votes")] Session session)
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanSubmit())
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                var addedSession = sessionRepository.AddSession(session);
                return RedirectToAction("Details", new {id = addedSession.SessionId});
            }

            return View(session);
        }

        public ActionResult Edit(int id = 0)
        {
            Session session = sessionRepository.Get(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Votes")] Session session)
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
            Session session = sessionRepository.Get(id);

            if (session == null)
            {
                return HttpNotFound();
            }

            var userProfile = userProfileRepository.GetUserProfileByUserName(session.SpeakerUserName);
            var displayModel = CreateDisplayModel(session, userProfile);
            return View(displayModel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            sessionRepository.DeleteSession(id);
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
    }
}
