using System;
using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Tests.Builders;
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
        public void TestThat_Index_CalculatesTheCorrectPercentage_ForTheDurationThroughTheVotingPeriod(
            int numberOfDaysSinceVotingOpened, int numberOfDaysVoting, int expectedPercentageCompletion)
        {
            var dataProvider = new DataProviderBuilder()
                                        .WithNumberOfDaysSinceVotingOpened(numberOfDaysSinceVotingOpened)
                                        .WithNumberOfDaysOfVoting(numberOfDaysVoting)
                                        .Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();

            Assert.That(model.VotingCompletePercentage, Is.EqualTo(expectedPercentageCompletion));
        }

        [TestCase]
        public void TestThat_Index_DoesNotCalculateGreaterThanOneHunderdPercent_ForTheDurationThroughTheVotingPeriod()
        {
            var dataProvider = new DataProviderBuilder()
                                        .WithNumberOfDaysSinceVotingOpened(25)
                                        .WithNumberOfDaysOfVoting(10)
                                        .Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();

            Assert.That(model.VotingCompletePercentage, Is.EqualTo(100));
        }

        [Test]
        public void TestThat_Index_DoesNotCalculateTheNumberOfDaysOfVotingPassedGreaterThanTheTotalNumberOfAllowableVotingDays()
        {
            var dataProvider = new DataProviderBuilder()
                                        .WithNumberOfDaysSinceVotingOpened(25)
                                        .WithNumberOfDaysOfVoting(10)
                                        .Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();

            Assert.That(model.NumberOfDaysOfVotingPassed, Is.EqualTo(10));
        }

        [Test]
        public void TestThat_Leaderboard_SetsTheCorrectNumberOfTotalSessionsForTheConference()
        {
            var conferenceLoader = new ConferenceLoaderBuilder().WithTotalNumberOfSessions(12).Build();
            var controller = new VotingControllerBuilder().WithConferenceLoader(conferenceLoader).Build();

            var model = controller.Index().GetViewModel<VotingStatsViewModel>();
            
            Assert.That(model.TotalNumberOfSessions, Is.EqualTo(12));
        }

        [Test]
        public void TestThat_Leaderboard_SetsTheHighestOccuringNumberOfVotesOnTheModel()
        {
            var sessions = new[] {new SessionLeaderBoardEntry {NumberOfVotes = 2}, new SessionLeaderBoardEntry {NumberOfVotes = 4}};
            var dataProvider = new DataProviderBuilder().WithLeaderboard(sessions).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.Leaderboard().GetViewModel<LeaderboardViewModel>();

            Assert.That(model.HighestVoteCount, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_Leaderboard_LimitsTheNumberOfSessions()
        {
            var dataProvider = new DataProviderBuilder().WithLeaderboard(new[] {new SessionLeaderBoardEntry()}).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            controller.Leaderboard(123);

            dataProvider.Received().GetLeaderBoard(123, Arg.Any<bool>());
        }

        [Test]
        public void TestThat_Leaderboard_ForbidsDuplicateSpeakers()
        {
            var dataProvider = new DataProviderBuilder().WithLeaderboard(new[] {new SessionLeaderBoardEntry()}).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            controller.Leaderboard(123, false);

            dataProvider.Received().GetLeaderBoard(Arg.Any<int>(), false);
        }

        [Test]
        public void TestThat_Leaderboard_SetsTheSessionsObtainedFromTheDataProviderOnTheModel()
        {
            var sessions = new[] {new SessionLeaderBoardEntry(), new SessionLeaderBoardEntry()};
            var dataProvider = new DataProviderBuilder().WithLeaderboard(sessions).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.Leaderboard().GetViewModel<LeaderboardViewModel>();

            CollectionAssert.AreEquivalent(sessions, model.Sessions);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void TestThat_LookupIPAddress_ThrowsAnException_WhenTheSuppliedIPAddressIsInavlid(string ipAddress)
        {
            var controller = new VotingControllerBuilder().Build();
            Assert.Throws<ArgumentException>(() => controller.LookupIPAddress(ipAddress));
        }

        [Test]
        public void TestThat_LookupIPAddress_ResolvesTheIPAddress_UsingTheDnsLookup()
        {
            var dnsLookup = new DnsLookupBuilder().Build();
            var controller = new VotingControllerBuilder().WithDnsLookup(dnsLookup).Build();

            controller.LookupIPAddress("1.2.3.4");

            dnsLookup.Received().Resolve("1.2.3.4");
        }

        [Test]
        public void TestThat_LookupIPAddress_ReturnsTheIPAddressResolvedByTheDnsLookup()
        {
            var dnsLookup = new DnsLookupBuilder().WithIPAddressResolvingTo("1.2.3.4", "some website").Build();
            var controller = new VotingControllerBuilder().WithDnsLookup(dnsLookup).Build();

            var result = controller.LookupIPAddress("1.2.3.4");

            Assert.That(result.Content, Is.EqualTo("some website"));
        }

        [Test]
        public void TestThat_IPAddresses_SetsTheHighestOccuringNumberOfVotesOnTheModel()
        {
            var votes = new[] {new VotesForIPAddressModel {NumberOfVotes = 2}, new VotesForIPAddressModel {NumberOfVotes = 4}};
            var dataProvider = new DataProviderBuilder().WithVotesForDistinctIPAddresses(votes).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.IPAddresses().GetViewModel<IPAddressStatsViewModel>();

            Assert.That(model.HighestVoteCount, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_IPAddresses_SetsTheSessionsObtainedFromTheDataProviderOnTheModel()
        {
            var votes = new[] {new VotesForIPAddressModel(), new VotesForIPAddressModel()};
            var dataProvider = new DataProviderBuilder().WithVotesForDistinctIPAddresses(votes).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.IPAddresses().GetViewModel<IPAddressStatsViewModel>();

            CollectionAssert.AreEquivalent(votes, model.IPAddresses);
        }

        [Test]
        public void TestThat_VotesPerDate_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var votes = new[] {new DateTimeVoteModel()};
            var dataProvider = new DataProviderBuilder().WithVotesPerDate(votes).Build();
            var chartDataConverter = new ChartDataConverterBuilder().Build();
            var controller = new VotingControllerBuilder()
                                    .WithDataProvider(dataProvider)
                                    .WithChartDataConverter(chartDataConverter)
                                    .Build();

            controller.VotesPerDate();

            chartDataConverter.Received().ToChartData(votes, Arg.Any<Func<DateTimeVoteModel, long>>());
        }

        [Test]
        public void TestThat_VotesPerDate_PassesTheCorrectChartDataToTheView()
        {
            long[][] chartData = new long[2][];
            var chartDataConverter = new ChartDataConverterBuilder().WithChartDataPerDate(chartData).Build();
            var controller = new VotingControllerBuilder().WithChartDataConverter(chartDataConverter).Build();

            var model = controller.VotesPerDate().GetViewModel<VotesPerDateViewModel>();

            CollectionAssert.AreEquivalent(chartData, model.DayByDay);
            CollectionAssert.AreEquivalent(chartData, model.Cumulative);
        }

        [Test]
        public void TestThat_VotesPerDay_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var votes = new[] { new DayOfWeekVoteModel() };
            var dataProvider = new DataProviderBuilder().WithVotesPerDay(votes).Build();
            var chartDataConverter = new ChartDataConverterBuilder().Build();
            var controller = new VotingControllerBuilder()
                                    .WithDataProvider(dataProvider)
                                    .WithChartDataConverter(chartDataConverter)
                                    .Build();

            controller.VotesPerDay();

            chartDataConverter.Received().ToChartData(votes);
        }

        [Test]
        public void TestThat_VotesPerDay_PassesTheCorrectChartDataToTheView()
        {
            long[][] chartData = new long[2][];
            var chartDataConverter = new ChartDataConverterBuilder().WithChartDataPerDay(chartData).Build();
            var controller = new VotingControllerBuilder().WithChartDataConverter(chartDataConverter).Build();

            var model = controller.VotesPerDay().GetViewModel<long[][]>();

            CollectionAssert.AreEquivalent(chartData, model);
        }

        [Test]
        public void TestThat_VotesPerHour_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var votes = new[] {new DateTimeVoteModel()};
            var dataProvider = new DataProviderBuilder().WithVotesPerHour(votes).Build();
            var chartDataConverter = new ChartDataConverterBuilder().Build();
            var controller = new VotingControllerBuilder()
                                    .WithDataProvider(dataProvider)
                                    .WithChartDataConverter(chartDataConverter)
                                    .Build();

            controller.VotesPerHour();

            chartDataConverter.Received().ToChartData(votes, Arg.Any<Func<DateTimeVoteModel, long>>());
        }

        [Test]
        public void TestThat_VotesPerHour_PassesTheCorrectChartDataToTheView()
        {
            long[][] chartData = new long[2][];
            var chartDataConverter = new ChartDataConverterBuilder().WithChartDataPerHour(chartData).Build();
            var controller = new VotingControllerBuilder().WithChartDataConverter(chartDataConverter).Build();

            var model = controller.VotesPerHour().GetViewModel<long[][]>();

            CollectionAssert.AreEquivalent(chartData, model);
        }

        [Test]
        public void TestThat_NumberOfUsersWhoHaveCastXVotes_UsesTheDataObtainedFromTheDataProvider_ToCreateTheChartData()
        {
            var voteCounts = new[] {new NumberOfUsersWithVotesModel()};
            var dataProvider = new DataProviderBuilder().WithNumberOfVotesCastCounts(voteCounts).Build();
            var chartDataConverter = new ChartDataConverterBuilder().Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider)
                                    .WithChartDataConverter(chartDataConverter)
                                    .Build();

            controller.NumberOfUsersWhoHaveCastXVotes();

            chartDataConverter.Received().ToChartData(voteCounts);
        }

        [Test]
        public void TestThat_NumberOfUsersWhoHaveCastXVotes_PassesTheCorrectChartDataToTheView()
        {
            long[][] chartData = new long[2][];
            var chartDataConverter = new ChartDataConverterBuilder().WithChartDataPerUser(chartData).Build();
            var controller = new VotingControllerBuilder().WithChartDataConverter(chartDataConverter).Build();

            var model = controller.NumberOfUsersWhoHaveCastXVotes().GetViewModel<long[][]>();

            CollectionAssert.AreEquivalent(chartData, model);
        }

        [Test]
        public void TestThat_VotersPerIPAddress_SetsTheHighestOccuringNumberOfVotersOnTheModel()
        {
            var voters = new[] {new IPAddressVoterModel {NumberOfVoters = 2}, new IPAddressVoterModel {NumberOfVoters = 4}};
            var dataProvider = new DataProviderBuilder().WithVotersForIPAddresses(voters).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.VotersPerIPAddress().GetViewModel<VotersPerIPAddressViewModel>();

            Assert.That(model.HighestNumberOfVoters, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_VotersPerIPAddress_SetsTheVotersObtainedFromTheDataProviderOnTheModel()
        {
            var voters = new[] {new IPAddressVoterModel(), new IPAddressVoterModel()};
            var dataProvider = new DataProviderBuilder().WithVotersForIPAddresses(voters).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.VotersPerIPAddress().GetViewModel<VotersPerIPAddressViewModel>();

            CollectionAssert.AreEquivalent(voters, model.IPAddressVoters);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void TestThat_VotesForIPAddress_ThrowsAnException_WhenTheSuppliedIPAddressIsInavlid(string ipAddress)
        {
            var controller = new VotingControllerBuilder().Build();
            Assert.Throws<ArgumentException>(() => controller.VotesForIPAddress(ipAddress));
        }

        [Test]
        public void TestThat_VotesForIPAddress_SetsTheIPAddressOnTheModel()
        {
            var votes = new[] {new CookieVoteModel(), new CookieVoteModel()};
            var dataProvider = new DataProviderBuilder().WithVotesForIPAddresses(votes).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.VotesForIPAddress("1.2.3.4").GetViewModel<VotesForIpAddressViewModel>();

            Assert.That(model.IPAddress, Is.EqualTo("1.2.3.4"));
        }

        [Test]
        public void TestThat_VotesForIPAddress_SetsTheHighestOccuringNumberOfVotesOnTheModel()
        {
            var votes = new[] {new CookieVoteModel {NumberOfVotes = 2}, new CookieVoteModel {NumberOfVotes = 4}};
            var dataProvider = new DataProviderBuilder().WithVotesForIPAddresses(votes).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.VotesForIPAddress("1.2.3.4").GetViewModel<VotesForIpAddressViewModel>();

            Assert.That(model.HighestNumberOfVotes, Is.EqualTo(4));
        }

        [Test]
        public void TestThat_VotesForIPAddress_SetsTheVotesObtainedFromTheDataProviderOnTheModel()
        {
            var votes = new[] {new CookieVoteModel(), new CookieVoteModel()};
            var dataProvider = new DataProviderBuilder().WithVotesForIPAddresses(votes).Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            var model = controller.VotesForIPAddress("1.2.3.4").GetViewModel<VotesForIpAddressViewModel>();

            CollectionAssert.AreEquivalent(votes, model.DistinctVotes);
        }

        [Test]
        public void TestThat_KnownUserVotes_GetsItsDataFromTheDataProvider()
        {
            var dataProvider = new DataProviderBuilder().Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            controller.KnownUserVotes();

            dataProvider.Received().GetKnownUserVotes();
        }

        [Test]
        public void TestThat_GetSessionsVotedForByKnownUser_ObtainsTheSessionsForTheSpecifiedUser()
        {
            var dataProvider = new DataProviderBuilder().Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            controller.GetSessionsVotedForByKnownUser(1234);

            dataProvider.Received().GetVotedForSessions(1234);
        }

        [Test]
        public void TestThat_AnonymousUserVotes_GetsItsDataFromTheDataProvider()
        {
            var dataProvider = new DataProviderBuilder().Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();

            controller.AnonymousUserVotes();

            dataProvider.Received().GetAnonymousUserVotes();
        }

        [Test]
        public void TestThat_GetSessionsVotedForByAnonymousUser_ObtainsTheSessionsForTheSpecifiedCookieId()
        {
            var dataProvider = new DataProviderBuilder().Build();
            var controller = new VotingControllerBuilder().WithDataProvider(dataProvider).Build();
            var cookieId = Guid.NewGuid();

            controller.GetSessionsVotedForByAnonymousUser(cookieId);

            dataProvider.Received().GetVotedForSessions(cookieId);
        }
    }
}
