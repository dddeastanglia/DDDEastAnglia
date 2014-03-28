using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        [HttpGet]
        public ActionResult Edit(string message = null)
        {
            DDDEAContext context = new DDDEAContext();
            UserProfile profile = context.UserProfiles.First(p => p.UserName == User.Identity.Name);
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

            if (ModelState.IsValid)
            {
                DDDEAContext context = new DDDEAContext();
                context.Entry(profile).State = EntityState.Modified;
                context.SaveChanges();
                message = "Your profile has been updated successfully.";
            }

            return RedirectToAction("Edit", new { Message = message });
        }
    }
}