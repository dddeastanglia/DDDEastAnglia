using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly ISessionLoaderFactory sessionLoaderFactory;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IUserProfileFilterFactory userProfileFilterFactory;

        public SpeakerController(IConferenceLoader conferenceLoader, ISessionLoaderFactory sessionLoaderFactory, IUserProfileRepository userProfileRepository, IUserProfileFilterFactory userProfileFilterFactory)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }

            if (sessionLoaderFactory == null)
            {
                throw new ArgumentNullException("sessionLoaderFactory");
            }

            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (userProfileFilterFactory == null)
            {
                throw new ArgumentNullException("userProfileFilterFactory");
            }

            this.conferenceLoader = conferenceLoader;
            this.sessionLoaderFactory = sessionLoaderFactory;
            this.userProfileRepository = userProfileRepository;
            this.userProfileFilterFactory = userProfileFilterFactory;
        }

        public ActionResult Index()
        {
            var conference = conferenceLoader.LoadConference();

            if (!conference.CanShowSpeakers())
            {
                return HttpNotFound();
            }

            var speakers = new List<SpeakerDisplayModel>();
            var speakerProfiles = userProfileRepository.GetAllUserProfiles();

            var sessionLoader = sessionLoaderFactory.Create(conference);
            var userProfileFilter = userProfileFilterFactory.Create(conference);
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
            var speakerProfile = userProfileRepository.GetUserProfileById(id);

            if (speakerProfile == null)
            {
                return HttpNotFound();
            }

            var conference = conferenceLoader.LoadConference();
            var sessionLoader = sessionLoaderFactory.Create(conference);
            var sessions = sessionLoader.LoadSessions(speakerProfile);

            if (id == 8823)
            {
                sessions = new[] { new SessionRepository().Get(2174) };
            }

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
                    Bio = userProfile.Bio,
                    GravatarUrl = userProfile.GravatarUrl(),
                    TwitterHandle = userProfile.TwitterHandle,
                    WebsiteUrl = userProfile.WebsiteUrl,
                    Sessions = userSessions
                };
        }
    }
}
