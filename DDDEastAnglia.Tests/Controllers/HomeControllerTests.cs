using DDDEastAnglia.Controllers;
using DDDEastAnglia.Models;
using DDDEastAnglia.Tests.Builders;
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

        [Test]
        public void Agenda_ShouldRedirectBackToIndex_WhenTheAgendaCannotBePublished()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithAgendaNotPublished()
                                        .Build();

            var controller = new HomeController(conferenceLoader);
            var viewName = controller.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Accommodation_ShouldRedirectBackToIndex_WhenRegistrationIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationNotOpen()
                                        .Build();

            var controller = new HomeController(conferenceLoader);
            var viewName = controller.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Register_ShouldRedirectBackToIndex_WhenRegistrationIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationNotOpen()
                                        .Build();

            var controller = new HomeController(conferenceLoader);
            var viewName = controller.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Preview_ShouldRedirectToTheHomePage_WhenTheConferenceIsNotInPreview()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .NotInPreview()
                                        .Build();

            var result = new HomeController(conferenceLoader).Preview();

            Assert.That(result.GetRedirectionUrl(), Is.EqualTo("~/"));
        }
    
        [Test]
        public void Closed_ShouldRedirectToTheHomePage_WhenTheConferenceIsNotClosed()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WhenNotClosed()
                                        .Build();

            var result = new HomeController(conferenceLoader).Closed();

            Assert.That(result.GetRedirectionUrl(), Is.EqualTo("~/"));
        }
    }
}
