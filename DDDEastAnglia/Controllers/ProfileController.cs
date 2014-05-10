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
                throw new ArgumentNullException("userProfileRepository");
            }
            
            this.userProfileRepository = userProfileRepository;
        }

        [HttpGet]
        public ActionResult Edit(string message = null)
        {
            UserProfile profile = userProfileRepository.GetUserProfileByUserName(User.Identity.Name);
            ViewBag.Message = message;
            return View(profile);
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfile profile)
        {
            string message = string.Empty;

            if (!string.IsNullOrWhiteSpace(profile.WebsiteUrl))
            {
                if (!Uri.IsWellFormedUriString(string.Format("http://{0}", profile.WebsiteUrl), UriKind.Absolute))
                {
                    ModelState.AddModelError(key: "WebsiteUrl", errorMessage: "The website url doesn't appear to be a valid URL!");
                    message = "The website url doesn't appear to be a valid URL!";
                }
            }

            if (profile.UserName != User.Identity.Name)
            {
                return new HttpUnauthorizedResult();
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
