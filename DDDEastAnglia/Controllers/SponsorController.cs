using System;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        private readonly SponsorModelQuery _sponsorModelQuery;
        private readonly SponsorLogoService _sponsorLogoService;

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
            _sponsorModelQuery = sponsorModelQuery;
            _sponsorLogoService = sponsorLogoService;
        }

        public ActionResult Logo(int sponsorId)
        {
            var image = _sponsorLogoService.Get(sponsorId);
            return new FileContentResult(image.Data, image.ContentType);
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return PartialView("_Sponsors", _sponsorModelQuery.Get());
        }
    }
}