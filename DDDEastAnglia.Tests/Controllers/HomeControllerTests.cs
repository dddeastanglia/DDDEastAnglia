using System.Web.Mvc;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using DDDEastAnglia.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class HomeControllerTests
    {
        [Test]
        public void SubmitSessionLinks_ShouldBeHidden_WhenSessionSubmissionIsNotOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanSubmit().Returns(false);

            var conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
            var controller = new HomeController(conferenceLoader);

            var result = (ViewResult) controller.About();
            var model = (AboutViewModel) result.Model;

            Assert.That(model.ShowSessionSubmissionLink, Is.False);
        }

        [Test]
        public void SubmitSessionLinks_ShouldBeShown_WhenSessionSubmissionIsOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanSubmit().Returns(true);

            var conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
            var controller = new HomeController(conferenceLoader);
            
            var result = (ViewResult) controller.About();
            var model = (AboutViewModel) result.Model;

            Assert.That(model.ShowSessionSubmissionLink, Is.True);
        }
    }
}
