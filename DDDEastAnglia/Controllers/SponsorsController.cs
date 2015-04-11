using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Controllers
{
    public class SponsorsController : Controller
    {
        public class SponsorSidebarModel
        {
            public string Name { get; set; }
            public string Logo { get; set; }
            public string Url { get; set; }
        }
        readonly ISponsorRepository _sponsorRepository;

        public SponsorsController(ISponsorRepository sponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var allSponsors = _sponsorRepository.GetAllSponsors().Select(x => new SponsorSidebarModel { Name = x.Name, Logo = "", Url = x.Url });
            return PartialView("_Sponsors", allSponsors);
        }
    }
}
