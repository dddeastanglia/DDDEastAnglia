using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        private readonly IViewModelQuery<IEnumerable<SponsorModel>> sponsorsQuery;
        private readonly ISponsorLogoService sponsorLogoService;

        public SponsorController(IViewModelQuery<IEnumerable<SponsorModel>> sponsorsQuery, ISponsorLogoService sponsorLogoService)
        {
            if (sponsorsQuery == null)
            {
                throw new ArgumentNullException(nameof(sponsorsQuery));
            }

            if (sponsorLogoService == null)
            {
                throw new ArgumentNullException(nameof(sponsorLogoService));
            }

            this.sponsorsQuery = sponsorsQuery;
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
            return PartialView("_Sponsors", sponsorsQuery.Get());
        }
    }
}
