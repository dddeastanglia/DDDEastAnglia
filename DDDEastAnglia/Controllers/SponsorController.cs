using System.Web.Mvc;
using DDDEastAnglia.Domain;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        private readonly SponsorModelQuery sponsorModelQuery;
        private readonly SponsorLogoService sponsorLogoService;

        public SponsorController(SponsorModelQuery sponsorModelQuery, SponsorLogoService sponsorLogoService)
        {
            this.sponsorModelQuery = sponsorModelQuery;
            this.sponsorLogoService = sponsorLogoService;
        }

        public ActionResult Logo(int sponsorId)
        {
            return sponsorLogoService.Get(sponsorId);
        }
        
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return PartialView("_Sponsors", sponsorModelQuery.Get());
        }
    }
}