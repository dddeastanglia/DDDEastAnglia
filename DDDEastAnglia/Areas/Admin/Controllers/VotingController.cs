using System;
using System.Linq;
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
        private readonly IDataProvider dataProvider;
        private readonly IDnsLookup dnsLookup;
        private readonly IChartDataConverter chartDataConverter;

        public VotingController() : this(DataProviderFactory.Create(), new DnsLookup(), new ChartDataConverter())
        {}

        public VotingController(IDataProvider dataProvider, IDnsLookup dnsLookup, IChartDataConverter chartDataConverter)
        {
            if (dataProvider == null)
            {
                throw new ArgumentNullException("dataProvider");
            }

            if (dnsLookup == null)
            {
                throw new ArgumentNullException("dnsLookup");
            }

            if (chartDataConverter == null)
            {
                throw new ArgumentNullException("chartDataConverter");
            }

            this.dataProvider = dataProvider;
            this.dnsLookup = dnsLookup;
            this.chartDataConverter = chartDataConverter;
        }

        public ActionResult Index()
        {
            int numberOfDaysOfVoting = dataProvider.GetNumberOfDaysOfVoting();
            int numberOfDaysSinceVotingOpened = dataProvider.GetNumberOfDaysSinceVotingOpened();
            int votingPercentComplete = (int) (numberOfDaysSinceVotingOpened * 1.0f / numberOfDaysOfVoting * 100);
            var model = new VotingStatsViewModel
                {
                    TotalVotes = dataProvider.GetTotalVoteCount(),
                    NumberOfUsersWhoHaveVoted = dataProvider.GetNumberOfUsersWhoHaveVoted(),
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
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException("ipAddress");
            }
            
            string hostName = dnsLookup.Resolve(ipAddress);
            return Content(hostName);
        }

        public ActionResult VotesPerDay()
        {
            var votesPerDay = dataProvider.GetVotesPerDay();
            var chartData = chartDataConverter.ToChartData(votesPerDay);
            return View(chartData);
        }

        public ActionResult VotesPerHour()
        {
            var votesPerHour = dataProvider.GetVotesPerHour();
            var chartData = chartDataConverter.ToChartData(votesPerHour);
            return View(chartData);
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
            if (string.IsNullOrWhiteSpace(ipAddress))
            {
                throw new ArgumentException("ipAddress");
            }
            
            var votesForIPAddress = dataProvider.GetVotesPerIPAddress(ipAddress);
            int highestNumberOfVotes = votesForIPAddress.Max(v => v.NumberOfVotes);
            var model = new VotesForIpAddressViewModel
                {
                    IPAddress = ipAddress,
                    HighestNumberOfVotes = highestNumberOfVotes,
                    DistinctVotes = votesForIPAddress
                };
            return View(model);
        }

        public ActionResult KnownUserVotes()
        {
            var knownUserVotes = dataProvider.GetKnownUserVotes();
            return View(knownUserVotes);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult GetSessionsVotedForByKnownUser(int userId)
        {
            var sessionsVotedFor = dataProvider.GetVotedForSessions(userId);
            return PartialView("_VotedForSessions", sessionsVotedFor);
        }

        public ActionResult AnonymousUserVotes()
        {
            var anonymousUserVotes = dataProvider.GetAnonymousUserVotes();
            return View(anonymousUserVotes);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult GetSessionsVotedForByAnonymousUser(Guid cookieId)
        {
            var sessionsVotedFor = dataProvider.GetVotedForSessions(cookieId);
            return PartialView("_VotedForSessions", sessionsVotedFor);
        }
    }
}
