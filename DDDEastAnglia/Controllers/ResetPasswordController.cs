using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IAccountLoginMethodQuery accountLoginMethodQuery;
        private readonly IResetPasswordThingy resetPasswordThingy;
        private readonly IResetPasswordEmailSender resetPasswordEmailSender;

        public ResetPasswordController(IUserProfileRepository userProfileRepository,
            IAccountLoginMethodQuery accountLoginMethodQuery,
            IResetPasswordThingy resetPasswordThingy,
            IResetPasswordEmailSender resetPasswordEmailSender)

        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (accountLoginMethodQuery == null)
            {
                throw new ArgumentNullException("accountLoginMethodQuery");
            }

            if (resetPasswordThingy == null)
            {
                throw new ArgumentNullException("resetPasswordThingy");
            }

            if (resetPasswordEmailSender == null)
            {
                throw new ArgumentNullException("resetPasswordEmailSender");
            }

            this.userProfileRepository = userProfileRepository;
            this.accountLoginMethodQuery = accountLoginMethodQuery;
            this.resetPasswordThingy = resetPasswordThingy;
            this.resetPasswordEmailSender = resetPasswordEmailSender;
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Start()
        {
            return View("Step1");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ResetPassword(ResetPasswordStepOneModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Step1", model);
            }

            UserProfile profile;

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                profile = userProfileRepository.GetUserProfileByUserName(model.UserName);
            }
            else if (!string.IsNullOrWhiteSpace(model.EmailAddress))
            {
                profile = userProfileRepository.GetUserProfileByEmailAddress(model.EmailAddress);
            }
            else
            {
                ModelState.AddModelError("", "A user name or email address must be specified.");
                return View("Step1");
            }

            if (profile == null)
            {
                // could't find the user, but don't want to give that away
                ViewBag.ShowAdditionalHelpMessage = false;
                return View("Step2");
            }

            var loginMethods = accountLoginMethodQuery.GetLoginMethods(profile.UserId);
            var dddeaLoginExists = loginMethods.Any(m => m.ProviderName == "dddea");

            if (dddeaLoginExists)
            {
                string passwordResetToken = resetPasswordThingy.GeneratePasswordResetToken(profile.UserName, 120);
                SendEmailToUser(profile.EmailAddress, passwordResetToken);
            }

            ViewBag.ShowAdditionalHelpMessage = !dddeaLoginExists;
            return View("Step2");
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult EmailConfirmation(string token)
        {
            return View("Step3", new ResetPasswordStepThreeModel { ResetToken = token });
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual ActionResult SaveNewPassword(ResetPasswordStepThreeModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Step3", model);
            }

            bool passwordWasReset = resetPasswordThingy.ResetPassword(model.ResetToken, model.Password);

            if (passwordWasReset)
            {
                return RedirectToAction("Complete", new { Message = "Your password has been changed successfully. Please log in using your new password." });
            }

            ModelState.AddModelError("", "There was an error resetting your password.");
            return View("Step3", model);
        }

        [HttpGet]
        [AllowAnonymous]
        public virtual ActionResult Complete(string message = null)
        {
            ViewBag.Message = message;
            return View("Step4");
        }

        private void SendEmailToUser(string emailAddress, string passwordResetToken)
        {
            string protocol = Request.Url.Scheme;
            string resetUrl = Url.Action("EmailConfirmation", "ResetPassword", new { token = passwordResetToken }, protocol);
            string htmlTemplatePath = Server.MapPath("~/ForgottenPasswordTemplate.html");
            string textTemplatePath = Server.MapPath("~/ForgottenPasswordTemplate.txt");
            resetPasswordEmailSender.SendEmail(htmlTemplatePath, textTemplatePath, emailAddress, resetUrl);
        }
    }
}
