using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Tests.Builders;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    public sealed class AdminHomeControllerTests
    {
        [Test]
        public void VotingLink_ShouldBeHidden_ByDefault()
        {
            var conferenceLoader = new ConferenceLoaderBuilder().Build();

            var model = new AdminHomeController(conferenceLoader)
                            .Index().GetViewModel<MenuViewModel>();
            
            Assert.That(model.ShowVotingStatsLink, Is.False);
        }

        [Test]
        public void VotingLink_ShouldBeHidden_WhenSessionSubmissionIsOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithSessionSubmissionOpen()
                                        .Build();

            var model = new AdminHomeController(conferenceLoader)
                            .Index().GetViewModel<MenuViewModel>();

            Assert.That(model.ShowVotingStatsLink, Is.False);
        }

        [Test]
        public void VotingLink_ShouldBeVisible_WhenVotingIsOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithVotingOpen()
                                        .Build();

            var model = new AdminHomeController(conferenceLoader)
                            .Index().GetViewModel<MenuViewModel>();

            Assert.That(model.ShowVotingStatsLink, Is.True);
        }

        [Test]
        public void VotingLink_ShouldBeVisible_WhenTheAgendaIsPublished()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithAgendaPublished()
                                        .Build();

            var model = new AdminHomeController(conferenceLoader)
                            .Index().GetViewModel<MenuViewModel>();

            Assert.That(model.ShowVotingStatsLink, Is.True);
        }

        [Test]
        public void VotingLink_ShouldBeVisible_WhenRegistrationIsOpen()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WithRegistrationOpen()
                                        .Build();

            var model = new AdminHomeController(conferenceLoader)
                            .Index().GetViewModel<MenuViewModel>();

            Assert.That(model.ShowVotingStatsLink, Is.True);
        }
    }
}
