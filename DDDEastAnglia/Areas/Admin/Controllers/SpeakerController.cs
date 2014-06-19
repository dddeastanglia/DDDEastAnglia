using System;
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
        private readonly ISpeakerRepository speakerRepository;

        public SpeakerController(ISpeakerRepository speakerRepository)
        {
            if (speakerRepository == null)
            {
                throw new ArgumentNullException("speakerRepository");
            }

            this.speakerRepository = speakerRepository;
        }

        public ActionResult Index()
        {
            var speakers = speakerRepository.GetAllSpeakerProfiles()
                                                .Select(CreateSpeakerModel)
                                                .OrderBy(s => s.UserName).ToList();
            return View(speakers);
        }
    
        private static SpeakerModel CreateSpeakerModel(SpeakerProfile profile)
        {
            return new SpeakerModel
            {
                UserId = profile.UserId,
                UserName = profile.UserName,
                Name = profile.Name,
                GravatarUrl = profile.GravatarUrl(),
                NewSpeaker = profile.NewSpeaker,
                SubmittedSessionCount = profile.NumberOfSubmittedSessions
            };
        }
    }
}