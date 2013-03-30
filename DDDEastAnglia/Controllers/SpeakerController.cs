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
                            GravatarUrl = string.Format("http://www.gravatar.com/avatar/{0}?s=50&d=identicon&r=pg",
                                CalculateGravatarUrl(speakerProfile.EmailAddress))
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

        private static string CalculateGravatarUrl(string emailAddress)
        {
            using( MD5 md5Hasher = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash.  
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(emailAddress));

                // Create a new Stringbuilder to collect the bytes  
                // and create a string.  
                var builder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string.  
                for (int i = 0; i < data.Length; i++)
                {
                    builder.Append(data[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

    }
}
