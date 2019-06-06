using System;
using System.Web.Mvc;
using System.Web.Security;
using DDDEastAnglia.DataAccess;
using WebMatrix.WebData;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IConferenceLoader conferenceLoader;

        public AccountController(IUserProfileRepository userProfileRepository, IConferenceLoader conferenceLoader)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }

            this.userProfileRepository = userProfileRepository;
            this.conferenceLoader = conferenceLoader;
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The username or password provided is incorrect.");
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);

                    // ensure the name and email address are set
                    var profile = userProfileRepository.GetUserProfileByUserName(model.UserName);
                    profile.Name = model.FullName;
                    profile.EmailAddress = model.EmailAddress;
                    userProfileRepository.UpdateUserProfile(profile);

                    var conference = conferenceLoader.LoadConference();
                    var registeredModel = new RegisteredModel
                    {
                        UserName = model.UserName,
                        CanSubmitSessions = conference.CanSubmit()
                    };
                    return View("Registered", registeredModel);
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword(string message = null)
        {
            ViewBag.Message = message;
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                try
                {
                    if (WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                    {
                        return RedirectToAction("ChangePassword", new { Message = "Your password has been changed successfully." });
                    }
                }
                catch (Exception)
                { }

                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed so redisplay the form
            return View(model);
        }

        [HttpGet]
        public ActionResult ManageLogins(string message = null)
        {
            ViewBag.Message = message;
            ViewBag.ReturnUrl = Url.Action("ManageLogins");
            return View("ManageLogins");
        }


        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "The username you have chosen already exists. Please choose a different username.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The username provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
