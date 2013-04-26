using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SpeakerController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();

        public ActionResult Index()
        {
            var speakers = new List<SpeakerDisplayModel>();
            var speakerProfiles = db.UserProfiles.ToList();

            foreach (var speakerProfile in speakerProfiles)
            {
                var speakersSessions = GetSessionsForSpeaker(speakerProfile);

                // exclude speakers that haven't submitted any sessions
                if (speakersSessions.Any())
                {
                    var speaker = CreateDisplayModel(speakerProfile, speakersSessions);
                    speakers.Add(speaker);
                }
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

            var sessions = GetSessionsForSpeaker(speakerProfile);
            var displayModel = CreateDisplayModel(speakerProfile, sessions);
            return View(displayModel);
        }

        private List<Session> GetSessionsForSpeaker(UserProfile speakerProfile)
        {
            return db.Sessions.Where(s => s.SpeakerUserName == speakerProfile.UserName).ToList();
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
