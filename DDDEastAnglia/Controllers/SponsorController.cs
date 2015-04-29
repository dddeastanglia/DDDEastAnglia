using System;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        private readonly SponsorModelQuery sponsorModelQuery;
        private readonly SponsorLogoService sponsorLogoService;

        public SponsorController(SponsorModelQuery sponsorModelQuery, SponsorLogoService sponsorLogoService)
        {
            if (sponsorModelQuery == null)
            {
                throw new ArgumentNullException("sponsorModelQuery");
            }
            if (sponsorLogoService == null)
            {
                throw new ArgumentNullException("sponsorLogoService");
            }
            this.sponsorModelQuery = sponsorModelQuery;
            this.sponsorLogoService = sponsorLogoService;
        }

        public ActionResult Logo(int sponsorId)
        {
            var image = sponsorLogoService.Get(sponsorId);
            return new FileContentResult(image.Data, image.ContentType);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return PartialView("_Sponsors", sponsorModelQuery.Get());
        }
    }
}