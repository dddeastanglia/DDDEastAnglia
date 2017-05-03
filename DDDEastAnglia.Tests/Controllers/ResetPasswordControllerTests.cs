using System;
using System.Web.Mvc;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Email;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class ResetPasswordControllerTests
    {
        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedContextIsNull()
        {
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            var resetPasswordEmailSender = Substitute.For<IResetPasswordEmailSender>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordController(null, resetPasswordThingy, resetPasswordEmailSender));
        }

        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedResetPasswordThingyIsNull()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var resetPasswordEmailSender = Substitute.For<IResetPasswordEmailSender>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordController(userProfileRepository, null, resetPasswordEmailSender));
        }

        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedResetPasswordEmailSenderIsNull()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            Assert.Throws<ArgumentNullException>(() => new ResetPasswordController(userProfileRepository, resetPasswordThingy, null));
        }

        [Test]
        public void TestThat_Start_BeginsAtStepOne()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());
            
            var result = (ViewResult) controller.Start();
            
            Assert.That(result.ViewName, Is.EqualTo("Step1"));
        }

        [Test]
        public void TestThat_ResetPassword_RedirectsBackToStepOne_WhenTheModelIsInvalid()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());
            controller.ModelState.AddModelError("", "invalid");

            var model = new ResetPasswordStepOneModel();
            var result = (ViewResult) controller.ResetPassword(model);

            Assert.That(result.ViewName, Is.EqualTo("Step1"));
        }

        [Test]
        public void TestThat_ResetPassword_AddsAValidationError_WhenTheModelHasAnInvalidUserNameAndEmailAddress()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());

            var model = new ResetPasswordStepOneModel {UserName = null, EmailAddress = null};
            controller.ResetPassword(model);

            Assert.That(controller.ModelState.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestThat_ResetPassword_RedirectsBackToStepOne_WhenTheModelHasAnInvalidUserNameAndEmailAddress()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());

            var model = new ResetPasswordStepOneModel {UserName = null, EmailAddress = null};
            var result = (ViewResult) controller.ResetPassword(model);

            Assert.That(result.ViewName, Is.EqualTo("Step1"));
        }

        [Test]
        public void TestThat_ResetPassword_RedirectsToStepTwo_WhenTheModelHasAValidUserName_ButTheUserProfileCouldNotBeFound()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());

            var model = new ResetPasswordStepOneModel {UserName = "bob"};
            var result = (ViewResult) controller.ResetPassword(model);

            Assert.That(result.ViewName, Is.EqualTo("Step2"));
        }

        [Test]
        public void TestThat_ResetPassword_RedirectsToStepTwo_WhenTheModelHasAValidEmailAddress_ButTheUserProfileCouldNotBeFound()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());

            var model = new ResetPasswordStepOneModel {EmailAddress = "bob@example.com"};
            var result = (ViewResult) controller.ResetPassword(model);

            Assert.That(result.ViewName, Is.EqualTo("Step2"));
        }

        [Test]
        public void TestThat_ResetPassword_GeneratesAPasswordResetTokenForTheUser_WhenAValidUserIsFound_FromAUserName()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileByUserName("bob").Returns(new UserProfile {UserName = "bob", EmailAddress = "bob@example.com"});
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            var controller = new ResetPasswordController(userProfileRepository, resetPasswordThingy, Substitute.For<IResetPasswordEmailSender>());
            controller.SetupWithHttpContextAndUrlHelper();

            var model = new ResetPasswordStepOneModel {UserName = "bob"};
            controller.ResetPassword(model);

            resetPasswordThingy.Received().GeneratePasswordResetToken("bob", Arg.Any<int>());
        }

        [Test]
        public void TestThat_ResetPassword_GeneratesAPasswordResetTokenForTheUser_WhenAValidUserIsFound_FromAnEmailAddress()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileByEmailAddress("bob@example.com").Returns(new UserProfile {UserName = "bob", EmailAddress = "bob@example.com"});
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            var controller = new ResetPasswordController(userProfileRepository, resetPasswordThingy, Substitute.For<IResetPasswordEmailSender>());
            controller.SetupWithHttpContextAndUrlHelper();

            var model = new ResetPasswordStepOneModel {EmailAddress = "bob@example.com"};
            controller.ResetPassword(model);

            resetPasswordThingy.Received().GeneratePasswordResetToken("bob", Arg.Any<int>());
        }

        [Test]
        public void TestThat_ResetPassword_SendsAnEmailToTheUser_WhenAValidUserIsFound_FromAUserName()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileByUserName("bob").Returns(new UserProfile {UserName = "bob", EmailAddress = "bob@example.com"});
            var resetPasswordEmailSender = Substitute.For<IResetPasswordEmailSender>();
            var controller = new ResetPasswordController(userProfileRepository, Substitute.For<IResetPasswordThingy>(), resetPasswordEmailSender);
            controller.SetupWithHttpContextAndUrlHelper();

            var model = new ResetPasswordStepOneModel {UserName = "bob"};
            controller.ResetPassword(model);

            resetPasswordEmailSender.Received().SendEmail(Arg.Any<string>(), Arg.Any<string>(), "bob@example.com", Arg.Any<string>());
        }

        [Test]
        public void TestThat_ResetPassword_SendsAnEmailToTheUser_WhenAValidUserIsFound_FromAnEmailAddress()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileByEmailAddress("bob@example.com").Returns(new UserProfile {UserName = "bob", EmailAddress = "bob@example.com"});
            var resetPasswordEmailSender = Substitute.For<IResetPasswordEmailSender>();
            var controller = new ResetPasswordController(userProfileRepository, Substitute.For<IResetPasswordThingy>(), resetPasswordEmailSender);
            controller.SetupWithHttpContextAndUrlHelper();

            var model = new ResetPasswordStepOneModel {EmailAddress = "bob@example.com"};
            controller.ResetPassword(model);

            resetPasswordEmailSender.Received().SendEmail(Arg.Any<string>(), Arg.Any<string>(), "bob@example.com", Arg.Any<string>());
        }

        [Test]
        public void TestThat_EmailConfirmation_RedirectsToTheCorrectView()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());
            
            var result = (ViewResult) controller.EmailConfirmation("token");
            
            Assert.That(result.ViewName, Is.EqualTo("Step3"));
        }

        [Test]
        public void TestThat_EmailConfirmation_PassesTheSuppliedToken_ToTheView()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());

            var result = (ViewResult) controller.EmailConfirmation("token");
            
            var model = (ResetPasswordStepThreeModel) result.Model;
            Assert.That(model.ResetToken, Is.EqualTo("token"));
        }

        [Test]
        public void TestThat_Complete_SetsTheSuppliedMessageOnTheView()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());

            var result = (ViewResult) controller.Complete("a message");

            Assert.That(result.ViewBag.Message, Is.EqualTo("a message"));
        }

        [Test]
        public void TestThat_SaveNewPassword_RedirectsToStepThree_WhenTheModelIsInvalid()
        {
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), Substitute.For<IResetPasswordThingy>(), Substitute.For<IResetPasswordEmailSender>());
            controller.ModelState.AddModelError("", "invalid");

            var result = (ViewResult) controller.SaveNewPassword(new ResetPasswordStepThreeModel());

            Assert.That(result.ViewName, Is.EqualTo("Step3"));
        }

        [Test]
        public void TestThat_SaveNewPassword_AddsAnError_WhenThePasswordCouldNotBeChanged()
        {
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            resetPasswordThingy.ResetPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), resetPasswordThingy, Substitute.For<IResetPasswordEmailSender>());

            controller.SaveNewPassword(new ResetPasswordStepThreeModel());

            Assert.That(controller.ModelState.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestThat_SaveNewPassword_RedirectsToStepThree_WhenThePasswordCouldNotBeChanged()
        {
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            resetPasswordThingy.ResetPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(false);
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), resetPasswordThingy, Substitute.For<IResetPasswordEmailSender>());

            var result = (ViewResult) controller.SaveNewPassword(new ResetPasswordStepThreeModel());

            Assert.That(result.ViewName, Is.EqualTo("Step3"));
        }

        [Test]
        public void TestThat_SaveNewPassword_RedirectsToCompletions_WhenThePasswordWasBeChanged()
        {
            var resetPasswordThingy = Substitute.For<IResetPasswordThingy>();
            resetPasswordThingy.ResetPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(true);
            var controller = new ResetPasswordController(Substitute.For<IUserProfileRepository>(), resetPasswordThingy, Substitute.For<IResetPasswordEmailSender>());

            var result = (RedirectToRouteResult) controller.SaveNewPassword(new ResetPasswordStepThreeModel());

            Assert.That(result.RouteValues["action"], Is.EqualTo("Complete"));
        }
    }
}
