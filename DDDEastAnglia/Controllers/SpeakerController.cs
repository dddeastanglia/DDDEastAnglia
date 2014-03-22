using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly IConferenceRepository conferenceRepository;
        private readonly ISessionLoaderFactory sessionLoaderFactory;
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IUserProfileFilterFactory userProfileFilterFactory;

        public SpeakerController(IConferenceRepository conferenceRepository, ISessionLoaderFactory sessionLoaderFactory, IUserProfileRepository userProfileRepository, IUserProfileFilterFactory userProfileFilterFactory)
        {
            if (conferenceRepository == null)
            {
                throw new ArgumentNullException("conferenceRepository");
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

            this.conferenceRepository = conferenceRepository;
            this.sessionLoaderFactory = sessionLoaderFactory;
            this.userProfileRepository = userProfileRepository;
            this.userProfileFilterFactory = userProfileFilterFactory;
        }

        public ActionResult Index()
        {
            var speakers = new List<SpeakerDisplayModel>();
            var speakerProfiles = userProfileRepository.GetAllUserProfiles();

            var sessionLoader = sessionLoaderFactory.Create(Get2013Conference());
            var userProfileFilter = userProfileFilterFactory.Create(Get2013Conference());
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

            var sessionLoader = sessionLoaderFactory.Create(Get2013Conference());
            var sessions = sessionLoader.LoadSessions(speakerProfile);
            var displayModel = CreateDisplayModel(speakerProfile, sessions);
            return View(displayModel);
        }

        private IConference Get2013Conference()
        {
            var dataConference = conferenceRepository.GetByEventShortName("DDDEA2013");
            return new ConferenceBuilder(new CalendarEntryBuilder()).Build(dataConference);
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
