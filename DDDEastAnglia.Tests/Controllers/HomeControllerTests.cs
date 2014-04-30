using DDDEastAnglia.Controllers;
using DDDEastAnglia.Models;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Controllers
{
    [TestFixture]
    public sealed class HomeControllerTests
    {
        [Test]
        public void SubmitSessionLinks_ShouldBeHidden_WhenSessionSubmissionIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithSessionSubmissionClosed()
                                        .Build();

            var model = new HomeController(conferenceLoader)
                            .About().GetViewModel<AboutViewModel>();

            Assert.That(model.ShowSessionSubmissionLink, Is.False);
        }

        [Test]
        public void SubmitSessionLinks_ShouldBeShown_WhenSessionSubmissionIsOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithSessionSubmissionOpen()
                                        .Build();

            var model = new HomeController(conferenceLoader)
                            .About().GetViewModel<AboutViewModel>();

            Assert.That(model.ShowSessionSubmissionLink, Is.True);
        }
    }
}
