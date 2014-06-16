using System.Net;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void Details_GetsTheCorrectUserDetails()
        {
            const int userId = 123;
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileById(userId).Returns(new UserProfile {UserId = userId});
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            var actionResult = controller.Details(userId);

            var model = actionResult.GetViewModel<UserProfile>();
            Assert.That(model.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void Details_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            var actionResult = controller.Details(123);

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Edit_GetsTheCorrectUserDetails()
        {
            const int userId = 123;
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileById(userId).Returns(new UserProfile { UserId = userId });
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            var actionResult = controller.Edit(userId);

            var model = actionResult.GetViewModel<UserProfile>();
            Assert.That(model.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void Edit_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            var actionResult = controller.Edit(123);

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void Edit_SavesTheUserProfileCorrectly()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);
            var userProfile = new UserProfile {UserName = "fred", Name = "Fred Bloggs", EmailAddress = "fred@example.com"};

            controller.Edit(userProfile);

            userProfileRepository.Received().UpdateUserProfile(userProfile);
        }

        [Test]
        public void Edit_DoesNotSaveTheUserProfile_WhenTheSubmittedDataIsInvalid()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);
            controller.CreateModelStateError();

            controller.Edit(new UserProfile());

            userProfileRepository.DidNotReceive().UpdateUserProfile(Arg.Any<UserProfile>());
        }

        [Test]
        public void Delete_GetsTheCorrectUserDetails()
        {
            const int userId = 123;
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            userProfileRepository.GetUserProfileById(userId).Returns(new UserProfile { UserId = userId });
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            var actionResult = controller.Delete(userId);

            var model = actionResult.GetViewModel<UserProfile>();
            Assert.That(model.UserId, Is.EqualTo(userId));
        }

        [Test]
        public void Delete_ReturnsA404_WhenTheUserCannotBeFound()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            var actionResult = controller.Delete(123);

            Assert.That(actionResult.GetHttpStatusCode(), Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void DeleteConfirmed_DeletesTheCorrectUser()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var sessionRepository = Substitute.For<ISessionRepository>();
            var controller = new UserController(userProfileRepository, sessionRepository);

            controller.DeleteConfirmed(123);

            userProfileRepository.Received().DeleteUserProfile(123);
        }
    }
}
