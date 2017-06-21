using System;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;

        public ProfileController(IUserProfileRepository userProfileRepository)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException(nameof(userProfileRepository));
            }

            this.userProfileRepository = userProfileRepository;
        }

        [HttpGet]
        [UserNameFilter("userName")]
        public ActionResult Edit(string userName, string message = null)
        {
            UserProfile profile = userProfileRepository.GetUserProfileByUserName(userName);
            ViewBag.Message = message;
            return View(profile);
        }

        [HttpPost]
        [UserNameFilter("userName")]
        public ActionResult UserProfile(string userName, UserProfile profile)
        {
            if (profile.UserName != userName)
            {
                return new HttpUnauthorizedResult();
            }

            string message = string.Empty;

            if (!string.IsNullOrWhiteSpace(profile.WebsiteUrl))
            {
                if (!Uri.IsWellFormedUriString($"http://{profile.WebsiteUrl}", UriKind.Absolute))
                {
                    message = "The website url doesn't appear to be a valid URL.";
                    ModelState.AddModelError(key: "WebsiteUrl", errorMessage: message);
                }
            }

            if (ModelState.IsValid)
            {
                userProfileRepository.UpdateUserProfile(profile);
                message = "Your profile has been updated successfully.";
            }

            return RedirectToAction("Edit", new { Message = message });
        }
    }
}
