using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Domain;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    public sealed class AdminHomeControllerTests
    {
        [Test]
        public void VotingLink_ShouldBeHidden_ByDefault()
        {
            var conference = Substitute.For<IConference>();

            var model = GetViewModel(conference);
            
            Assert.That(model.ShowVotingStatsLink, Is.False);
        }

        [Test]
        public void VotingLink_ShouldBeHidden_WhenSessionSubmissionIsOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanSubmit().Returns(true);

            var model = GetViewModel(conference);

            Assert.That(model.ShowVotingStatsLink, Is.False);
        }

        [Test]
        public void VotingLink_ShouldBeHidden_WhenVotingIsOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanVote().Returns(true);

            var model = GetViewModel(conference);

            Assert.That(model.ShowVotingStatsLink, Is.True);
        }

        [Test]
        public void VotingLink_ShouldBeHidden_WhenTheAgendaIsPublished()
        {
            var conference = Substitute.For<IConference>();
            conference.CanPublishAgenda().Returns(true);

            var model = GetViewModel(conference);

            Assert.That(model.ShowVotingStatsLink, Is.True);
        }

        [Test]
        public void VotingLink_ShouldBeHidden_WhenRegistrationIsOpen()
        {
            var conference = Substitute.For<IConference>();
            conference.CanRegister().Returns(true);

            var model = GetViewModel(conference);

            Assert.That(model.ShowVotingStatsLink, Is.True);
        }

        private MenuViewModel GetViewModel(IConference conference)
        {
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            conferenceLoader.LoadConference().Returns(conference);
            var controller = new AdminHomeController(conferenceLoader);

            var result = (ViewResult) controller.Index();
            var model = (MenuViewModel) result.Model;
            return model;
        }
    }
}
