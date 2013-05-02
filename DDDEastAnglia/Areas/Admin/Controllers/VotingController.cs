using System.Configuration;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
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
                    NumberOfDaysOfVoting = dataProvider.GetNumberOfDaysOfVoting(),
                    NumberOfDaysOfVotingPassed = dataProvider.GetNumberOfDaysSinceVotingOpened(),
                    NumberOfDaysOfVotingRemaining = dataProvider.GetNumberOfDaysUntilVotingCloses(),
                    VotingStartDate = dataProvider.GetVotingStartDate(),
                    VotingEndDate = dataProvider.GetVotingEndDate(),
                };
            return View(model);
        }

        public ActionResult Leaderboard()
        {
            var leaderboardSessions = dataProvider.GetLeaderBoard();
            var highestVoteCount = leaderboardSessions.Max(s => s.NumberOfVotes);
            var model = new LeaderboardViewModel
                {
                    HighestVoteCount = highestVoteCount,
                    Sessions = leaderboardSessions
                };
            return View(model);
        }
    }
}
