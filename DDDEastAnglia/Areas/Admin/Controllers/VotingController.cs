using System;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Mvc.Attributes;
using DDDEastAnglia.VotingData;
using System.Linq;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class VotingController : Controller
    {
        private readonly DataProvider dataProvider;

        public VotingController()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DDDEastAnglia"];
            dataProvider = new DataProvider(connectionStringSettings.ConnectionString);
        }

        public ActionResult Index()
        {
            var model = new VotingStatsViewModel
                {
                    TotalVotes = dataProvider.GetTotalVoteCount(),
                    NumberOfUsersWhoHaveVoted = dataProvider.GetnumberOfUsersWhoHaveVoted(),
                    NumberOfDaysOfVoting = dataProvider.GetNumberOfDaysOfVoting(),
                    NumberOfDaysOfVotingPassed = dataProvider.GetNumberOfDaysSinceVotingOpened(),
                    NumberOfDaysOfVotingRemaining = dataProvider.GetNumberOfDaysUntilVotingCloses(),
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
            string hostName;

            try
            {
                var ipHostEntry = Dns.GetHostEntry(ipAddress);
                hostName = ipHostEntry.HostName;
            }
            catch
            {
                hostName = "[unknown]";
            }

            return Content(hostName);
        }
    }
}
