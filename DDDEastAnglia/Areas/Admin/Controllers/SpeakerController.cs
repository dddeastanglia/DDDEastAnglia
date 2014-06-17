using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class SpeakerController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly ISessionRepository sessionRepository;

        public SpeakerController(IUserProfileRepository userProfileRepository, ISessionRepository sessionRepository)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (sessionRepository == null)
            {
                throw new ArgumentNullException("sessionRepository");
            }
            
            this.userProfileRepository = userProfileRepository;
            this.sessionRepository = sessionRepository;
        }

        public ActionResult Index()
        {
            var sessionCountsPerSpeaker = sessionRepository.GetAllSessions()
                                                        .GroupBy(s => s.SpeakerUserName)
                                                        .Where(g => g.Any())
                                                        .ToDictionary(g => g.Key, g => g.Count());

            var speakerUserNames = new HashSet<string>(sessionCountsPerSpeaker.Select(p => p.Key));

            var speakers = userProfileRepository.GetAllUserProfiles()
                                                .Where(u => speakerUserNames.Contains(u.UserName))
                                                .Select(CreateUserModel)
                                                .OrderBy(u => u.UserName).ToList();
            foreach (var speaker in speakers)
            {
                int sessionCount;
                sessionCountsPerSpeaker.TryGetValue(speaker.UserName, out sessionCount);
                speaker.SubmittedSessionCount = sessionCount;
            }

            return View(speakers);
        }
    
        private static SpeakerModel CreateUserModel(UserProfile profile)
        {
            return new SpeakerModel
            {
                UserId = profile.UserId,
                UserName = profile.UserName,
                Name = profile.Name,
                NewSpeaker = profile.NewSpeaker,
                GravatarUrl = profile.GravatarUrl()
            };
        }
    }
}