using System;
using System.Collections.Generic;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData;
using DDDEastAnglia.VotingData.Models;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Admin
{
    [TestFixture]
    public sealed class VotingControllerTests
    {
        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedConferenceLoaderIsNull()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var dnsLookup = Substitute.For<IDnsLookup>();
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            Assert.Throws<ArgumentNullException>(() => new VotingController(null, dataProvider, dnsLookup, chartDataConverter));
        }

        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedDataProviderIsNull()
        {
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            var dnsLookup = Substitute.For<IDnsLookup>();
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            Assert.Throws<ArgumentNullException>(() => new VotingController(conferenceLoader, null, dnsLookup, chartDataConverter));
        }

        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedDnsLookupIsNull()
        {
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            var dataProvider = Substitute.For<IDataProvider>();
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            Assert.Throws<ArgumentNullException>(() => new VotingController(conferenceLoader, dataProvider, null, chartDataConverter));
        }

        [Test]
        public void TestThat_Ctor_ThrowsAnException_WhenTheSuppliedChartDataConverterIsNull()
        {
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            var dataProvider = Substitute.For<IDataProvider>();
            var dnsLookup = Substitute.For<IDnsLookup>();
            Assert.Throws<ArgumentNullException>(() => new VotingController(conferenceLoader, dataProvider, dnsLookup, null));
        }

        [TestCase(10, 10, 100)]
        [TestCase(6, 10, 60)]
        [TestCase(1, 4, 25)]
        [TestCase(1, 6, 16)]
        public void TestThat_Index_CalculatesTheCorrectPercentage_ForTheDurationThroughTheVotingPeriod(int numberOfDaysSinceVotingOpened, int numberOfDaysVoting, int expectedPercentageCompletion)
        {
            var dataProvider = Substitute.For<IDataProvider>();
            dataProvider.GetNumberOfDaysSinceVotingOpened().Returns(numberOfDaysSinceVotingOpened);
            dataProvider.GetNumberOfDaysOfVoting().Returns(numberOfDaysVoting);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();

            Assert.That(model.VotingCompletePercentage, Is.EqualTo(expectedPercentageCompletion));
        }
        
        [TestCase]
        public void TestThat_Index_DoesNotCalculateGreaterThanOneHunderdPercent_ForTheDurationThroughTheVotingPeriod()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            dataProvider.GetNumberOfDaysSinceVotingOpened().Returns(25);
            dataProvider.GetNumberOfDaysOfVoting().Returns(10);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();

            Assert.That(model.VotingCompletePercentage, Is.EqualTo(100));
        }

        [Test]
        public void TestThat_Index_DoesNotCalculateTheNumberOfDaysOfVotingPassedGreaterThanTheTotalNumberOfAllowableVotingDays()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            dataProvider.GetNumberOfDaysSinceVotingOpened().Returns(25);
            dataProvider.GetNumberOfDaysOfVoting().Returns(10);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();

            Assert.That(model.NumberOfDaysOfVotingPassed, Is.EqualTo(10));
        }

        [Test]
        public void TestThat_Leaderboard_SetsTheHighestOccuringNumberOfVotesOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var sessions = new[] {new SessionLeaderBoardEntry {NumberOfVotes = 2}, new SessionLeaderBoardEntry {NumberOfVotes = 4}};
            dataProvider.GetLeaderBoard(Arg.Any<int>(), Arg.Any<bool>()).Returns(sessions);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.Leaderboard().GetViewModel<LeaderboardViewModel>();
            
            Assert.That(model.HighestVoteCount, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_Leaderboard_LimitsTheNumberOfSessions()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            dataProvider.GetLeaderBoard(Arg.Any<int>(), Arg.Any<bool>()).Returns(new[] {new SessionLeaderBoardEntry()});
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());
            
            controller.Leaderboard(123);

            dataProvider.Received().GetLeaderBoard(123, Arg.Any<bool>());
        }

        [Test]
        public void TestThat_Leaderboard_ForbidsDuplicateSpeakers()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            dataProvider.GetLeaderBoard(Arg.Any<int>(), Arg.Any<bool>()).Returns(new[] {new SessionLeaderBoardEntry()});
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());
            
            controller.Leaderboard(123, false);

            dataProvider.Received().GetLeaderBoard(Arg.Any<int>(), false);
        }

        [Test]
        public void TestThat_Leaderboard_SetsTheSessionsObtainedFromTheDataProviderOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var sessions = new[] {new SessionLeaderBoardEntry(), new SessionLeaderBoardEntry()};
            dataProvider.GetLeaderBoard(Arg.Any<int>(), Arg.Any<bool>()).Returns(sessions);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.Leaderboard().GetViewModel<LeaderboardViewModel>();
            
            CollectionAssert.AreEquivalent(sessions, model.Sessions);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void TestThat_LookupIPAddress_ThrowsAnException_WhenTheSuppliedIPAddressIsInavlid(string ipAddress)
        {
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), Substitute.For<IDataProvider>(), Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());
            Assert.Throws<ArgumentException>(() => controller.LookupIPAddress(ipAddress));
        }

        [Test]
        public void TestThat_LookupIPAddress_ResolvesTheIPAddress_UsingTheDnsLookup()
        {
            var dnsLookup = Substitute.For<IDnsLookup>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), Substitute.For<IDataProvider>(), dnsLookup, Substitute.For<IChartDataConverter>());

            controller.LookupIPAddress("1.2.3.4");

            dnsLookup.Received().Resolve("1.2.3.4");
        }

        [Test]
        public void TestThat_LookupIPAddress_ReturnsTheIPAddressResolvedByTheDnsLookup()
        {
            var dnsLookup = Substitute.For<IDnsLookup>();
            dnsLookup.Resolve("1.2.3.4").Returns("some website");
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), Substitute.For<IDataProvider>(), dnsLookup, Substitute.For<IChartDataConverter>());

            var result = controller.LookupIPAddress("1.2.3.4");

            Assert.That(result.Content, Is.EqualTo("some website"));
        }

        [Test]
        public void TestThat_IPAddresses_SetsTheHighestOccuringNumberOfVotesOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new VotesForIPAddressModel {NumberOfVotes = 2}, new VotesForIPAddressModel {NumberOfVotes = 4}};
            dataProvider.GetDistinctIPAddresses().Returns(votes);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.IPAddresses().GetViewModel<IPAddressStatsViewModel>();
            
            Assert.That(model.HighestVoteCount, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_IPAddresses_SetsTheSessionsObtainedFromTheDataProviderOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new VotesForIPAddressModel(), new VotesForIPAddressModel()};
            dataProvider.GetDistinctIPAddresses().Returns(votes);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.IPAddresses().GetViewModel<IPAddressStatsViewModel>();
            
            CollectionAssert.AreEquivalent(votes, model.IPAddresses);
        }

        [Test]
        public void TestThat_VotesPerDay_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new DateTimeVoteModel()};
            dataProvider.GetVotesPerDay().Returns(votes);
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), chartDataConverter);

            controller.VotesPerDay();

            chartDataConverter.Received().ToChartData(votes);
        }

        [Test]
        public void TestThat_VotesPerDay_PassesTheCorrectChartDataToTheView()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            long[][] chartData = new long[2][];
            chartDataConverter.ToChartData(Arg.Any<IList<DateTimeVoteModel>>()).Returns(chartData);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), chartDataConverter);

            var model = controller.VotesPerDay().GetViewModel<VotesPerDayViewModel>();

            CollectionAssert.AreEquivalent(chartData, model.DayByDay);
            CollectionAssert.AreEquivalent(chartData, model.Cumulative);
        }

        [Test]
        public void TestThat_VotesPerHour_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new DateTimeVoteModel()};
            dataProvider.GetVotesPerHour().Returns(votes);
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), chartDataConverter);

            controller.VotesPerHour();

            chartDataConverter.Received().ToChartData(votes);
        }

        [Test]
        public void TestThat_VotesPerHour_PassesTheChartDataToTheView()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            long[][] chartData = new long[2][];
            chartDataConverter.ToChartData(Arg.Any<IList<DateTimeVoteModel>>()).Returns(chartData);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), chartDataConverter);

            var model = controller.VotesPerHour().GetViewModel<long[][]>();

            CollectionAssert.AreEquivalent(chartData, model);
        }

        [Test]
        public void TestThat_NumberOfUsersWhoHaveCastXVotes_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var voteCounts = new[] {new NumberOfUsersWithVotesModel()};
            dataProvider.GetNumberOfVotesCastCounts().Returns(voteCounts);
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), chartDataConverter);

            controller.NumberOfUsersWhoHaveCastXVotes();

            chartDataConverter.Received().ToChartData(voteCounts);
        }

        [Test]
        public void TestThat_NumberOfUsersWhoHaveCastXVotes_PassesTheChartDataToTheView()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var chartDataConverter = Substitute.For<IChartDataConverter>();
            long[][] chartData = new long[2][];
            chartDataConverter.ToChartData(Arg.Any<IList<NumberOfUsersWithVotesModel>>()).Returns(chartData);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), chartDataConverter);

            var model = controller.NumberOfUsersWhoHaveCastXVotes().GetViewModel<long[][]>();

            CollectionAssert.AreEquivalent(chartData, model);
        }

        [Test]
        public void TestThat_VotersPerIPAddress_SetsTheHighestOccuringNumberOfVotersOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var voters = new[] {new IPAddressVoterModel {NumberOfVoters = 2}, new IPAddressVoterModel {NumberOfVoters = 4}};
            dataProvider.GetVotersPerIPAddress().Returns(voters);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.VotersPerIPAddress().GetViewModel<VotersPerIPAddressViewModel>();
            
            Assert.That(model.HighestNumberOfVoters, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_VotersPerIPAddress_SetsTheVotersObtainedFromTheDataProviderOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var voters = new[] {new IPAddressVoterModel(), new IPAddressVoterModel()};
            dataProvider.GetVotersPerIPAddress().Returns(voters);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.VotersPerIPAddress().GetViewModel<VotersPerIPAddressViewModel>();
            
            CollectionAssert.AreEquivalent(voters, model.IPAddressVoters);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void TestThat_VotesForIPAddress_ThrowsAnException_WhenTheSuppliedIPAddressIsInavlid(string ipAddress)
        {
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), Substitute.For<IDataProvider>(), Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());
            Assert.Throws<ArgumentException>(() => controller.VotesForIPAddress(ipAddress));
        }

        [Test]
        public void TestThat_VotesForIPAddress_SetsTheIPAddressOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new CookieVoteModel(), new CookieVoteModel()};
            dataProvider.GetVotesPerIPAddress(Arg.Any<string>()).Returns(votes);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.VotesForIPAddress("1.2.3.4").GetViewModel<VotesForIpAddressViewModel>();
            
            Assert.That(model.IPAddress, Is.EqualTo("1.2.3.4"));
        }

        [Test]
        public void TestThat_VotesForIPAddress_SetsTheHighestOccuringNumberOfVotesOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new CookieVoteModel {NumberOfVotes = 2}, new CookieVoteModel {NumberOfVotes = 4}};
            dataProvider.GetVotesPerIPAddress(Arg.Any<string>()).Returns(votes);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.VotesForIPAddress("1.2.3.4").GetViewModel<VotesForIpAddressViewModel>();
            
            Assert.That(model.HighestNumberOfVotes, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_VotesForIPAddress_SetsTheVotesObtainedFromTheDataProviderOnTheModel()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var votes = new[] {new CookieVoteModel(), new CookieVoteModel()};
            dataProvider.GetVotesPerIPAddress(Arg.Any<string>()).Returns(votes);
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            var model = controller.VotesForIPAddress("1.2.3.4").GetViewModel<VotesForIpAddressViewModel>();
            
            CollectionAssert.AreEquivalent(votes, model.DistinctVotes);
        }

        [Test]
        public void TestThat_KnownUserVotes_GetsItsDataFromTheDataProvider()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            controller.KnownUserVotes();

            dataProvider.Received().GetKnownUserVotes();
        }

        [Test]
        public void TestThat_GetSessionsVotedForByKnownUser_ObtainsTheSessionsForTheSpecifiedUser()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            controller.GetSessionsVotedForByKnownUser(1234);

            dataProvider.Received().GetVotedForSessions(1234);
        }

        [Test]
        public void TestThat_AnonymousUserVotes_GetsItsDataFromTheDataProvider()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());

            controller.AnonymousUserVotes();

            dataProvider.Received().GetAnonymousUserVotes();
        }

        [Test]
        public void TestThat_GetSessionsVotedForByAnonymousUser_ObtainsTheSessionsForTheSpecifiedCookieId()
        {
            var dataProvider = Substitute.For<IDataProvider>();
            var controller = new VotingController(Substitute.For<IConferenceLoader>(), dataProvider, Substitute.For<IDnsLookup>(), Substitute.For<IChartDataConverter>());
            var cookieId = Guid.NewGuid();

            controller.GetSessionsVotedForByAnonymousUser(cookieId);

            dataProvider.Received().GetVotedForSessions(cookieId);
        }
    }
}
