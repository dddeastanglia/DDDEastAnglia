using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.Helpers.LoginMethods;
using DDDEastAnglia.Helpers.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IBuild<LoginMethod, LoginMethodViewModel> loginMethodViewModelBuilder;
        private readonly ExternalLoginsDirectory externalLoginsDirectory;
        private readonly IOAuthClientInfo oAuthClientInfo;

        public AccountController(IUserProfileRepository userProfileRepository, 
            IBuild<LoginMethod, LoginMethodViewModel> loginMethodViewModelBuilder,
            ExternalLoginsDirectory externalLoginsDirectory, 
            IOAuthClientInfo oAuthClientInfo)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (loginMethodViewModelBuilder == null)
            {
                throw new ArgumentNullException("loginMethodViewModelBuilder");
            }

            if (externalLoginsDirectory == null)
            {
                throw new ArgumentNullException("externalLoginsDirectory");
            }

            if (oAuthClientInfo == null)
            {
                throw new ArgumentNullException("oAuthClientInfo");
            }

            this.userProfileRepository = userProfileRepository;
            this.loginMethodViewModelBuilder = loginMethodViewModelBuilder;
            this.externalLoginsDirectory = externalLoginsDirectory;
            this.oAuthClientInfo = oAuthClientInfo;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

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

                    return View("Registered", model);
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
            ViewBag.HasLocalAccount = oAuthClientInfo.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.Message = message;
            return View("ChangePassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            bool hasLocalAccount = oAuthClientInfo.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalAccount = hasLocalAccount;

            if (hasLocalAccount)
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
            }

            // If we got this far, something failed so redisplay the form
            return View(model);
        }

        [HttpGet]
        public ActionResult ManageLogins(string message = null)
        {
            ViewBag.HasLocalAccount = oAuthClientInfo.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.Message = message;
            ViewBag.ReturnUrl = Url.Action("ManageLogins");
            return View("ManageLogins");
        }

        [ChildActionOnly]
        public ActionResult ExternalLogins(string returnUrl)
        {
            var allExternalLogins = externalLoginsDirectory.GetAllAvailable();
            var usersExternalLogins = externalLoginsDirectory.GetForUser(User.Identity.Name).ToList();
            var usersProviderToUserId = usersExternalLogins.ToDictionary(l => l.ProviderName, l => l.ProviderUserId);

            foreach (var externalLogin in allExternalLogins)
            {
                string providerUserId;

                if (usersProviderToUserId.TryGetValue(externalLogin.ProviderName, out providerUserId))
                {
                    externalLogin.ProviderUserId = providerUserId;
                }
            }

            bool hasLocalAccount = oAuthClientInfo.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.DisableRemoveButtons = hasLocalAccount || usersExternalLogins.Count() > 1;
            var viewModels = allExternalLogins.Select(loginMethodViewModelBuilder.Build);
            return PartialView("_ExternalLoginsPartial", viewModels);
        }
        
        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginMethods()
        {
            var externalLogins = externalLoginsDirectory.GetAllAvailable();
            var viewModels = externalLogins.Select(loginMethodViewModelBuilder.Build);
            return PartialView("_ExternalLoginMethodsPartial", viewModels);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginButtons(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            var externalLogins = externalLoginsDirectory.GetAllAvailable();
            var viewModels = externalLogins.Select(loginMethodViewModelBuilder.Build);
            return PartialView("_ExternalLoginButtonsPartial", viewModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisassociateLogin(string name, string provider, string providerUserId)
        {
            string ownerAccount = oAuthClientInfo.GetUserName(provider, providerUserId);
            string message = null;

            if (ownerAccount == User.Identity.Name)
            {
                bool hasLocalAccount = oAuthClientInfo.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                var externalLogins = oAuthClientInfo.GetAccountsFromUserName(User.Identity.Name);

                if (hasLocalAccount || externalLogins.Count > 1)
                {
                    oAuthClientInfo.DeleteAccount(provider, providerUserId);
                    message = string.Format("Your {0} login has been removed successfully.", name);
                }
            }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            AuthenticationResult result = oAuthClientInfo.VerifyAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));

            if (!result.IsSuccessful)
            {
                return RedirectToAction("ExternalLoginFailure");
            }

            if (oAuthClientInfo.Login(result.Provider, result.ProviderUserId, createPersistentCookie: false))
            {
                return RedirectToLocal(returnUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                // If the current user is logged in add the new account
                oAuthClientInfo.CreateOrUpdateAccount(result.Provider, result.ProviderUserId, User.Identity.Name);
                return RedirectToLocal(returnUrl);
            }

            // User is new, ask for their desired membership name
            string loginData = oAuthClientInfo.SerializeProviderUserId(result.Provider, result.ProviderUserId);
            ViewBag.ProviderDisplayName = oAuthClientInfo.GetOAuthClientData(result.Provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View("ExternalLoginConfirmation", new RegisterExternalLoginModel { UserName = result.UserName, ExternalLoginData = loginData });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider;
            string providerUserId;

            if (User.Identity.IsAuthenticated || !oAuthClientInfo.TryDeserializeProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
            {
                return RedirectToAction("ManageLogins");
            }

            if (ModelState.IsValid)
            {
                // Insert a new user into the database
                UserProfile user = userProfileRepository.GetUserProfileByUserName(model.UserName);

                // Check if user already exists
                if (user == null)
                {
                    // Insert name into the profile table
                    var userProfile = new UserProfile { UserName = model.UserName, Name = model.Name, EmailAddress = model.EmailAddress };
                    userProfileRepository.AddUserProfile(userProfile);

                    oAuthClientInfo.CreateOrUpdateAccount(provider, providerUserId, model.UserName);
                    oAuthClientInfo.Login(provider, providerUserId, createPersistentCookie: false);

                    return RedirectToLocal(returnUrl);
                }

                ModelState.AddModelError("UserName", ErrorCodeToString(MembershipCreateStatus.DuplicateUserName));
            }

            ViewBag.ProviderDisplayName = oAuthClientInfo.GetOAuthClientData(provider).DisplayName;
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
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

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
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
