using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.VotingData;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VotingController : Controller
    {
        private readonly DataProvider dataProvider;
        private readonly DnsLookup dnsLookup;

        public VotingController()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DDDEastAnglia"];
            var queryRunner = new QueryRunner(connectionStringSettings.ConnectionString);
            dataProvider = new DataProvider(queryRunner);
            dnsLookup = new DnsLookup();
        }

        public ActionResult Index()
        {
            int numberOfDaysOfVoting = dataProvider.GetNumberOfDaysOfVoting();
            int numberOfDaysSinceVotingOpened = dataProvider.GetNumberOfDaysSinceVotingOpened();
            int votingPercentComplete = (int) (numberOfDaysSinceVotingOpened * 1.0f / numberOfDaysOfVoting * 100);
            var model = new VotingStatsViewModel
                {
                    TotalVotes = dataProvider.GetTotalVoteCount(),
                    NumberOfUsersWhoHaveVoted = dataProvider.GetnumberOfUsersWhoHaveVoted(),
                    NumberOfDaysOfVoting = numberOfDaysOfVoting,
                    NumberOfDaysOfVotingPassed = numberOfDaysSinceVotingOpened,
                    NumberOfDaysOfVotingRemaining = dataProvider.GetNumberOfDaysUntilVotingCloses(),
                    VotingCompletePercentage = votingPercentComplete,
                    VotingStartDate = dataProvider.GetVotingStartDate(),
                    VotingEndDate = dataProvider.GetVotingEndDate(),
                };
            return View(model);
        }

        public ActionResult Leaderboard(int limit = int.MaxValue)
        {
            var leaderboardSessions = dataProvider.GetLeaderBoard(limit);
            var highestVoteCount = leaderboardSessions.Max(s => s.NumberOfVotes);
            var model = new LeaderboardViewModel
                {
                    HighestVoteCount = highestVoteCount,
                    Sessions = leaderboardSessions
                };
            return View(model);
        }

        public ActionResult IPAddresses()
        {
            var ipAddresses = dataProvider.GetDistinctIPAddresses();
            var highestVoteCount = ipAddresses.Max(s => s.NumberOfVotes);
            var model = new IPAddressStatsViewModel
                {
                    HighestVoteCount = highestVoteCount,
                    IPAddresses = ipAddresses
                };
            return View(model);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ContentResult LookupIPAddress(string ipAddress)
        {
            string hostName = dnsLookup.Resolve(ipAddress);
            return Content(hostName);
        }

        public ActionResult VotesPerDay()
        {
            var votesPerDay = dataProvider.GetVotesPerDay();
            var chartData = GetChartData(votesPerDay);
            return View(chartData);
        }

        public ActionResult VotesPerHour()
        {
            var votesPerHour = dataProvider.GetVotesPerHour();
            var chartData = GetChartData(votesPerHour);
            return View(chartData);
        }

        private long[][] GetChartData(IList<DateTimeVoteModel> voteData)
        {
            long[][] chartData = new long[voteData.Count][];

            foreach (var item in voteData.Select((v, i) => new {Index = i, Vote = v}))
            {
                var vote = item.Vote;
                long javascriptTimestamp = vote.Date.GetJavascriptTimestamp();
                chartData[item.Index] = new[] {javascriptTimestamp, vote.VoteCount};
            }

            return chartData;
        }

        public ActionResult VotersPerIPAddress()
        {
            var votersPerIPAddress = dataProvider.GetVotersPerIPAddress();
            int highestNumberOfVoters = votersPerIPAddress.Max(v => v.NumberOfVoters);
            var model = new VotersPerIPAddressViewModel
                {
                    HighestNumberOfVoters = highestNumberOfVoters,
                    IPAddressVoters = votersPerIPAddress
                };
            return View(model);
        }

        public ActionResult VotesForIPAddress(string ipAddress)
        {
            var votesForIPAddress = dataProvider.GetVotesPerCookieIPAddress(ipAddress);
            int highestNumberOfVotes = votesForIPAddress.Max(v => v.NumberOfVotes);
            var model = new VotesForIpAddressViewModel
                {
                    IPAddress = ipAddress,
                    HighestNumberOfVotes = highestNumberOfVotes,
                    DistinctVotes = votesForIPAddress
                };
            return View(model);
        }
    }
}
