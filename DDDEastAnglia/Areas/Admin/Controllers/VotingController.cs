using System.Linq;
using System.Configuration;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.VotingData;

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
            dataProvider = new DataProvider(connectionStringSettings.ConnectionString);
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
            var model = new VotesPerDateTimeViewModel { Votes = votesPerDay };
            return View(model);
        }

        public ActionResult VotesPerHour()
        {
            var votesPerHour = dataProvider.GetVotesPerHour();
            var model = new VotesPerDateTimeViewModel { Votes = votesPerHour };
            return View(model);
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
