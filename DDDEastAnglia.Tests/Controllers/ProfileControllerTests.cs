using System.Web.Mvc;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class ProfileControllerTests
    {
        [Test]
        public void CannotSaveAProfileForADifferentUser()
        {
            var userProfileRepository = Substitute.For<IUserProfileRepository>();
            var controller = new ProfileController(userProfileRepository);

            var result = controller.UserProfile("fred", new UserProfile {UserName = "bob"});
            Assert.That(result, Is.InstanceOf<HttpUnauthorizedResult>());
        }
    }
}
