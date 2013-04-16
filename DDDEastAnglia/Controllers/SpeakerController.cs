using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public partial class SpeakerController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();

        public virtual ActionResult Index()
        {
            var speakers = new List<SpeakerDisplayModel>();
            var speakerProfiles = db.UserProfiles.ToList();

            foreach (var speakerProfile in speakerProfiles)
            {
                int sessionCount = db.Sessions.Count(s => s.SpeakerUserName == speakerProfile.UserName);

                if (sessionCount > 0)
                {
                    SpeakerDisplayModel speaker = new SpeakerDisplayModel
                        {
                            Name = speakerProfile.Name,
                            Bio = speakerProfile.Bio,
                            TwitterHandle = speakerProfile.TwitterHandle,
                            WebsiteUrl = speakerProfile.WebsiteUrl,
                            GravatarUrl = speakerProfile.GravitarUrl()
                        };

                    var speakerSessions = db.Sessions.Where(s => s.SpeakerUserName == speakerProfile.UserName).ToList();
                    
                    foreach (var speakerSession in speakerSessions)
                    {
                        speaker.Sessions.Add(speakerSession.SessionId, speakerSession.Title);
                    }
                    
                    speakers.Add(speaker);
                }
            }

            return View(speakers);
        }

        public virtual ActionResult Details(int id = 0)
        {
            var userProfile = db.UserProfiles.Find(id);
        
            if (userProfile == null)
            {
                return HttpNotFound();
            }

            var sessions = db.Sessions.Where(s => s.SpeakerUserName == userProfile.UserName);
            var displayModel = CreateDisplayModel(userProfile, sessions);
            return View(displayModel);
        }

        private SpeakerDisplayModel CreateDisplayModel(UserProfile userProfile, IEnumerable<Session> sessions)
        {
            return new SpeakerDisplayModel
                {
                    Name = userProfile.Name,
                    GravatarUrl = userProfile.GravitarUrl(),
                    Bio = userProfile.Bio,
                    TwitterHandle = userProfile.TwitterHandle,
                    WebsiteUrl = userProfile.WebsiteUrl,
                    Sessions = sessions.ToDictionary(s => s.SessionId, s => s.Title)
                };
        }
    }
}
