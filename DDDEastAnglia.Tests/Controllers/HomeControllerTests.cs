using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
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

            var homeController = CreateSUT(conferenceLoader);

            var model = homeController.About().GetViewModel<AboutViewModel>();

            Assert.That(model.ShowSessionSubmissionLink, Is.False);
        }

        [Test]
        public void SubmitSessionLinks_ShouldBeShown_WhenSessionSubmissionIsOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithSessionSubmissionOpen()
                                        .Build();
            var homeController = CreateSUT(conferenceLoader);

            var model = homeController.About().GetViewModel<AboutViewModel>();

            Assert.That(model.ShowSessionSubmissionLink, Is.True);
        }

        [Test]
        public void Agenda_ShouldRedirectBackToIndex_WhenTheAgendaCannotBePublished()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithAgendaNotPublished()
                                        .Build();
            var homeController = CreateSUT(conferenceLoader);

            var viewName = homeController.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Accommodation_ShouldRedirectBackToIndex_WhenRegistrationIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationNotOpen()
                                        .Build();
            var homeController = CreateSUT(conferenceLoader);

            var viewName = homeController.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Register_ShouldRedirectBackToIndex_WhenRegistrationIsNotOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationNotOpen()
                                        .Build();
            var homeController = CreateSUT(conferenceLoader);

            var viewName = homeController.Agenda().GetRedirectionViewName();

            Assert.That(viewName, Is.EqualTo("Index"));
        }

        [Test]
        public void Preview_ShouldRedirectToTheHomePage_WhenTheConferenceIsNotInPreview()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .NotInPreview()
                                        .Build();
            var homeController = CreateSUT(conferenceLoader);

            var result = homeController.Preview();

            Assert.That(result.GetRedirectionUrl(), Is.EqualTo("~/"));
        }

        [Test]
        public void Closed_ShouldRedirectToTheHomePage_WhenTheConferenceIsNotClosed()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WhenNotClosed()
                                        .Build();
            var homeController = CreateSUT(conferenceLoader);

            var result = homeController.Closed();

            Assert.That(result.GetRedirectionUrl(), Is.EqualTo("~/"));
        }

        private HomeController CreateSUT(IConferenceLoader conferenceLoader)
        {
            var sponsorModelQuery = new SponsorModelQuery(new InMemorySponsorRepository());
            return new HomeController(conferenceLoader, sponsorModelQuery);
        }
    }
}
