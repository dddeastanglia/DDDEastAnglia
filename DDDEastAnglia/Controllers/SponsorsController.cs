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
            public int SponsorId { get; set; }
            public string Url { get; set; }
        }
        readonly ISponsorRepository sponsorRepository;

        public SponsorsController(ISponsorRepository sponsorRepository)
        {
            this.sponsorRepository = sponsorRepository;
        }

        public ActionResult Logo(int sponsorId)
        {
            var sponsor = sponsorRepository.GetSponsor(sponsorId);
            return File(sponsor.Logo, "image/png");
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var allSponsors = sponsorRepository
                .GetAllSponsors()
                .OrderByDescending(s => s.SponsorshipAmount).ThenBy(s => s.PaymentDate)
                .Select(x => new SponsorSidebarModel { Name = x.Name, SponsorId = x.SponsorId, Url = x.Url })
                ;
            return PartialView("_Sponsors", allSponsors);
        }
    }
}
