using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IAccountLoginMethodQuery accountLoginMethodQuery;
        private readonly ISessionRepository sessionRepository;

        public UserController(IUserProfileRepository userProfileRepository, IAccountLoginMethodQuery accountLoginMethodQuery, ISessionRepository sessionRepository)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (accountLoginMethodQuery == null)
            {
                throw new ArgumentNullException("accountLoginMethodQuery");
            }

            if (sessionRepository == null)
            {
                throw new ArgumentNullException("sessionRepository");
            }
            
            this.userProfileRepository = userProfileRepository;
            this.accountLoginMethodQuery = accountLoginMethodQuery;
            this.sessionRepository = sessionRepository;
        }

        public ActionResult Index()
        {
            var users = userProfileRepository.GetAllUserProfiles()
                                             .Select(CreateUserModel)
                                             .OrderBy(u => u.UserName).ToList();

            var sessionCountsPerUser = sessionRepository.GetAllSessions()
                                                        .GroupBy(s => s.SpeakerUserName)
                                                        .ToDictionary(g => g.Key, g => g.Count());

            foreach (var user in users)
            {
                int sessionCount;
                sessionCountsPerUser.TryGetValue(user.UserName, out sessionCount);
                user.SubmittedSessionCount = sessionCount;
            }

            return View(users);
        }

        public ActionResult Details(int id)
        {
            var userProfile = userProfileRepository.GetUserProfileById(id);

            if (userProfile == null)
            {
                return HttpNotFound();
            }

            userProfile.LoginMethods = accountLoginMethodQuery.GetLoginMethods(id);
            return View(userProfile);
        }

        public ActionResult Edit(int id)
        {
            var userProfile = userProfileRepository.GetUserProfileById(id);
            return userProfile == null ? (ActionResult) HttpNotFound() : View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                userProfileRepository.UpdateUserProfile(userProfile);
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        public ActionResult Delete(int id)
        {
            var userProfile = userProfileRepository.GetUserProfileById(id);
            return userProfile == null ? (ActionResult) HttpNotFound() : View(userProfile);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userProfileRepository.DeleteUserProfile(id);
            return RedirectToAction("Index");
        }

        private static UserModel CreateUserModel(UserProfile profile)
        {
            return new UserModel
                {
                    UserId = profile.UserId,
                    UserName = profile.UserName,
                    Name = profile.Name,
                    EmailAddress = profile.EmailAddress,
                    MobilePhone = profile.MobilePhone,
                    WebsiteUrl = profile.WebsiteUrl,
                    TwitterHandle = profile.TwitterHandle,
                    Bio = profile.Bio,
                    NewSpeaker = profile.NewSpeaker,
                    GravatarUrl = profile.GravatarUrl()
                };
        }
    }
}
