using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();
        private readonly IConference conference;
        private readonly ISessionLoaderFactory sessionLoaderFactory;
        private readonly IUserProfileFilterFactory userProfileFilterFactory;

        public SpeakerController() 
            : this(Factory.GetConferenceRepository().GetByEventShortName("DDDEA2013"),
                    new SessionLoaderFactory(), new UserProfileFilterFactory())
        {}

        public SpeakerController(IConference conference, ISessionLoaderFactory sessionLoaderFactory, IUserProfileFilterFactory userProfileFilterFactory)
        {
            if (conference == null)
            {
                throw new ArgumentNullException("conference");
            }

            if (sessionLoaderFactory == null)
            {
                throw new ArgumentNullException("sessionLoaderFactory");
            }

            if (userProfileFilterFactory == null)
            {
                throw new ArgumentNullException("userProfileFilterFactory");
            }
            
            this.conference = conference;
            this.sessionLoaderFactory = sessionLoaderFactory;
            this.userProfileFilterFactory = userProfileFilterFactory;
        }

        public ActionResult Index()
        {
            var speakers = new List<SpeakerDisplayModel>();
            var speakerProfiles = db.UserProfiles.ToList();

            var context = new DDDEAContextWrapper(db);
            var sessionLoader = sessionLoaderFactory.Create(conference, context);
            var userProfileFilter = userProfileFilterFactory.Create(conference, context);
            var speakersWhoHaveSubmittedSessions = userProfileFilter.FilterProfiles(speakerProfiles);

            foreach (var speakerProfile in speakersWhoHaveSubmittedSessions)
            {
                var speakersSessions = sessionLoader.LoadSessions(speakerProfile);
                var speaker = CreateDisplayModel(speakerProfile, speakersSessions);
                speakers.Add(speaker);
            }

            speakers.Sort(new SpeakerDisplayModelComparer());
            return View(speakers);
        }

        public ActionResult Details(int id = 0)
        {
            var speakerProfile = db.UserProfiles.Find(id);

            if (speakerProfile == null)
            {
                return HttpNotFound();
            }

            var sessionLoader = sessionLoaderFactory.Create(conference, new DDDEAContextWrapper(db));
            var sessions = sessionLoader.LoadSessions(speakerProfile);
            var displayModel = CreateDisplayModel(speakerProfile, sessions);
            return View(displayModel);
        }

        private SpeakerDisplayModel CreateDisplayModel(UserProfile userProfile, IEnumerable<Session> sessions)
        {
            var isCurrentUser = Request.IsAuthenticated && userProfile.UserName == User.Identity.Name;
            var userSessions = sessions.ToDictionary(s => s.SessionId, s => s.Title);

            return new SpeakerDisplayModel
                {
                    IsCurrentUser = isCurrentUser,
                    Name = userProfile.Name,
                    GravatarUrl = userProfile.GravatarUrl(),
                    Bio = userProfile.Bio,
                    TwitterHandle = userProfile.TwitterHandle,
                    WebsiteUrl = userProfile.WebsiteUrl,
                    Sessions = userSessions,
                    Username = userProfile.UserName
                };
        }
    }
}
