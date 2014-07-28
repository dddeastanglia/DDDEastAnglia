using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.VotingData;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VotingController : Controller
    {
        private readonly IConferenceLoader conferenceLoader;
        private readonly IDataProvider dataProvider;
        private readonly IDnsLookup dnsLookup;
        private readonly IChartDataConverter chartDataConverter;

        public VotingController(IConferenceLoader conferenceLoader, IDataProvider dataProvider, IDnsLookup dnsLookup, IChartDataConverter chartDataConverter)
        {
            if (conferenceLoader == null)
            {
                throw new ArgumentNullException("conferenceLoader");
            }

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

            this.conferenceLoader = conferenceLoader;
            this.dataProvider = dataProvider;
            this.dnsLookup = dnsLookup;
            this.chartDataConverter = chartDataConverter;
        }

        public ActionResult Index()
        {
            int numberOfSessions = conferenceLoader.LoadConference().TotalNumberOfSessions;
            int numberOfDaysOfVoting = dataProvider.GetNumberOfDaysOfVoting();
            int numberOfDaysOfVotingPassed = Math.Min(numberOfDaysOfVoting, dataProvider.GetNumberOfDaysSinceVotingOpened());
            int votingPercentComplete = (int)(numberOfDaysOfVotingPassed * 1.0f / numberOfDaysOfVoting * 100);
            var model = new VotingStatsViewModel
            {
                TotalVotes = dataProvider.GetTotalVoteCount(),
                NumberOfUsersWhoHaveVoted = dataProvider.GetNumberOfUsersWhoHaveVoted(),
                NumberOfDaysOfVoting = numberOfDaysOfVoting,
                NumberOfDaysOfVotingPassed = numberOfDaysOfVotingPassed,
                NumberOfDaysOfVotingRemaining = dataProvider.GetNumberOfDaysUntilVotingCloses(),
                VotingCompletePercentage = votingPercentComplete,
                VotingStartDate = dataProvider.GetVotingStartDate(),
                VotingEndDate = dataProvider.GetVotingEndDate(),
                TotalNumberOfSessions = numberOfSessions
            };
            return View(model);
        }

        public ActionResult Leaderboard(int limit = int.MaxValue, bool allowDuplicateSpeakers = true)
        {
            var leaderboardSessions = dataProvider.GetLeaderBoard(limit, allowDuplicateSpeakers);
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
            var votesPerDayData = chartDataConverter.ToChartData(votesPerDay);

            var cumulativeVotesPerDayData = WorkOutCumulativeVotes(votesPerDay);

            var viewModel = new VotesPerDayViewModel { DayByDay = votesPerDayData, Cumulative = cumulativeVotesPerDayData };
            return View(viewModel);
        }

        private long[][] WorkOutCumulativeVotes(IEnumerable<DateTimeVoteModel> votesPerDay)
        {
            int totalVotes = 0;
            var cumulativeVotesPerDay = new List<DateTimeVoteModel>();

            foreach (var dateTimeVoteModel in votesPerDay)
            {
                totalVotes += dateTimeVoteModel.VoteCount;
                var model = new DateTimeVoteModel { Date = dateTimeVoteModel.Date, VoteCount = totalVotes };
                cumulativeVotesPerDay.Add(model);
            }

            var cumulativeVotesPerDayData = chartDataConverter.ToChartData(cumulativeVotesPerDay);
            return cumulativeVotesPerDayData;
        }

        public ActionResult VotesPerHour()
        {
            var votesPerHour = dataProvider.GetVotesPerHour();
            var chartData = chartDataConverter.ToChartData(votesPerHour);
            return View(chartData);
        }

        public ActionResult NumberOfUsersWhoHaveCastXVotes()
        {
            ViewBag.NumberOfSessions = conferenceLoader.LoadConference().TotalNumberOfSessions;
            var numberOfVotesCastCounts = dataProvider.GetNumberOfVotesCastCounts();
            var chartData = chartDataConverter.ToChartData(numberOfVotesCastCounts);
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

        public ActionResult DuplicateVotes()
        {
            var duplicateVotes = dataProvider.GetDuplicateVotes();
            return View(duplicateVotes);
        }

        public ActionResult VotersForSessions()
        {
            var leaderboardSessions = dataProvider.GetLeaderBoard(int.MaxValue, true);
            return View(leaderboardSessions);
        }

        [AllowCrossSiteJson]
        public ActionResult GetUsersWhoVotedForSession(int sessionId)
        {
            var votersForSession = dataProvider.GetVotersForSession(sessionId);
            var sortedVoters = votersForSession.OrderBy(v => v.IsAnonymous).ThenBy(v => v.UserIdentifier);
            return PartialView("_UsersVotedForSession", sortedVoters);
        }

        public ActionResult IPAddressesThatVotedForSessions()
        {
            var leaderboardSessions = dataProvider.GetLeaderBoard(int.MaxValue, true);
            return View(leaderboardSessions);
        }

        [HttpPost]
        [AllowCrossSiteJson]
        public ActionResult GetIPAddressesWhoVotedForSession(int sessionId)
        {
            var ipAddressesThatVotedForSession = dataProvider.GetIPAddressesThatVotedForSession(sessionId);
            return PartialView("_IPAddressesVotedForSession", ipAddressesThatVotedForSession);
        }
    }
}
