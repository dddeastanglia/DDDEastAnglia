using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Helpers.AppSettings;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Helpers.Email.SendGrid;
using DDDEastAnglia.Helpers.Email.Smtp;
using DDDEastAnglia.Helpers.File;
using DDDEastAnglia.Models;
using WebMatrix.WebData;

namespace DDDEastAnglia.Controllers
{
    public class ResetPasswordController : Controller
    {
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
            DDDEAContext context = new DDDEAContext();

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                profile = context.UserProfiles.FirstOrDefault(p => p.UserName == model.UserName);

            }
            else if (!string.IsNullOrWhiteSpace(model.EmailAddress))
            {
                profile = context.UserProfiles.FirstOrDefault(p => p.EmailAddress == model.EmailAddress);
            }
            else
            {
                ModelState.AddModelError("", "A user name or email address must be specified.");
                return View("Step1");
            }

            if (profile == null)
            {
                ModelState.AddModelError("", "Could not reset password.");
                return View("Step1");
            }

            string passwordResetToken = WebSecurity.GeneratePasswordResetToken(model.UserName, 120);
            SendEmailToUser(profile.EmailAddress, passwordResetToken);

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

            bool passwordWasReset = WebSecurity.ResetPassword(model.ResetToken, model.Password);

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
            string resetUrl = Url.Action("EmailConfirmation", "ResetPassword", new { token = passwordResetToken }, Request.Url.Scheme);
            string htmlTemplatePath = Server.MapPath("~/ForgottenPasswordTemplate.html");
            string textTemplatePath = Server.MapPath("~/ForgottenPasswordTemplate.txt");

            var hostSettingsProvider = new SmtpHostSettingsProvider(new WebConfigurationAppSettingsProvider());
            var emailSender = new SendGridEmailSender(hostSettingsProvider.GetSettings());
            var resetPasswordEmailSender = new ResetPasswordEmailSender(emailSender, new SendGridMessageFactory(), new FileContentsProvider());
            resetPasswordEmailSender.SendEmail(htmlTemplatePath, textTemplatePath, emailAddress, resetUrl);
        }    
    }
}
