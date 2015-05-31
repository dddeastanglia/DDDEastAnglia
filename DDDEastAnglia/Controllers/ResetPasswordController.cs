using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Services.Messenger.Email.Templates;
using System;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly IUserProfileRepository userProfileRepository;
        private readonly IResetPasswordService resetPasswordService;
        private readonly EmailMessengerFactory emailMessengerFactory;

        public ResetPasswordController(IUserProfileRepository userProfileRepository, IResetPasswordService resetPasswordService, EmailMessengerFactory emailMessengerFactory)
        {
            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }

            if (resetPasswordService == null)
            {
                throw new ArgumentNullException("resetPasswordService");
            }

            if (emailMessengerFactory == null)
            {
                throw new ArgumentNullException("emailMessengerFactory");
            }

            this.userProfileRepository = userProfileRepository;
            this.resetPasswordService = resetPasswordService;
            this.emailMessengerFactory = emailMessengerFactory;
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
                return View("Step2");
            }

            string passwordResetToken = resetPasswordService.GeneratePasswordResetToken(profile.UserName, 120);
            SendEmailToUser(profile, passwordResetToken);

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

            bool passwordWasReset = resetPasswordService.ResetPassword(model.ResetToken, model.Password);

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

        private void SendEmailToUser(UserProfile profile, string passwordResetToken)
        {
            string protocol = Request.Url.Scheme;
            string resetUrl = Url.Action("EmailConfirmation", "ResetPassword", new { token = passwordResetToken }, protocol);

            var mailTemplate = PasswordResetMailTemplate.Create(resetUrl);
            emailMessengerFactory.CreateEmailMessenger(mailTemplate).Notify(profile);
        }
    }
}
