using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;
using SendGridMail;
using SendGridMail.Transport;
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

            string passwordResetToken;

            if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                DDDEAContext context = new DDDEAContext();
                var profile = context.UserProfiles.FirstOrDefault(p => p.UserName == model.UserName);

                if (profile == null)
                {
                    ModelState.AddModelError("", "Could not reset password.");
                    return View("Step1");
                }

                passwordResetToken = WebSecurity.GeneratePasswordResetToken(model.UserName);
            }
            else if (!string.IsNullOrWhiteSpace(model.EmailAddress))
            {
                DDDEAContext context = new DDDEAContext();
                var profile = context.UserProfiles.FirstOrDefault(p => p.EmailAddress == model.EmailAddress);

                if (profile == null)
                {
                    ModelState.AddModelError("", "Could not reset password.");
                    return View("Step1");
                }
                
                passwordResetToken = WebSecurity.GeneratePasswordResetToken(profile.UserName);
            }
            else
            {
                ModelState.AddModelError("", "A user name or email address must be specified.");
                return View("Step1");
            }

            SendEmailToUser(passwordResetToken);

            return View("Step2", new ResetPasswordStepTwoModel { Token = passwordResetToken });
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

        private void SendEmailToUser(string passwordResetToken)
        {
            string resetUrl = Url.Action("EmailConfirmation", "ResetPassword", new { token = passwordResetToken }, Request.Url.Scheme);
            
            var from = new MailAddress("site@dddeastanglia.com");
            var to = new[] { new MailAddress("test@adrianbanks.co.uk") };
            var subject = "DDD East Anglia Password Reset";
            var html = string.Format("<a href=\"{0}\">Reset password</a>", resetUrl);
            var text = resetUrl;

            var smtpUsername = WebConfigurationManager.AppSettings["SMTPUsername"];
            var smtpPassword = WebConfigurationManager.AppSettings["SMTPPassword"];
            var smtpHost = WebConfigurationManager.AppSettings["SMTPHost"];
            var smtpPort = int.Parse(WebConfigurationManager.AppSettings["SMTPPort"]);

            SendGrid message = SendGrid.GetInstance(from, to, new MailAddress[0], new MailAddress[0], subject, html, text);
            var credentials = new NetworkCredential(smtpUsername, smtpPassword);
            SMTP.GetInstance(credentials, smtpHost, smtpPort).Deliver(message);
        }    
    }
}