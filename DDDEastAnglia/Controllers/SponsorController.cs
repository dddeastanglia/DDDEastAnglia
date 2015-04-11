using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        private readonly SponsorModelQuery sponsorModelQuery;
        private readonly ISponsorRepository sponsorRepository;

        public SponsorController(SponsorModelQuery sponsorModelQuery, ISponsorRepository sponsorRepository)
        {
            this.sponsorModelQuery = sponsorModelQuery;
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
            return PartialView("_Sponsors", sponsorModelQuery.Get());
        }
    }
}
