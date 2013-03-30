using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Security.Cryptography;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public partial class SpeakerController : Controller
    {
        private DDDEAContext db = new DDDEAContext();
        //
        // GET: /Speaker/

        public virtual ActionResult Index()
        {
            List<SpeakerDisplayModel> speakers = new List<SpeakerDisplayModel>();

            List<UserProfile> speakerProfiles = db.UserProfiles.ToList();

            foreach (UserProfile speakerProfile in speakerProfiles)
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
                            GravatarUrl = string.Format("http://www.gravatar.com/avatar/{0}?s=160&d=identicon&r=pg",
                                this.calculateGravatarUrl(speakerProfile.EmailAddress))
                        };

                    List<Session> speakerSessions = db.Sessions.Where(s => s.SpeakerUserName == speakerProfile.UserName).ToList();
                    foreach (var speakerSession in speakerSessions)
                    {
                        speaker.Sessions.Add(speakerSession.SessionId, speakerSession.Title);
                    }
                    speakers.Add(speaker);
                }
            }
            return View(speakers);
        }

        private string calculateGravatarUrl(string emailAddress)
        {
            byte[] emailBytes = Encoding.UTF8.GetBytes(emailAddress);
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] hashedBytes = provider.ComputeHash(emailBytes);
            return Convert.ToBase64String(hashedBytes);
        }

    }
}
