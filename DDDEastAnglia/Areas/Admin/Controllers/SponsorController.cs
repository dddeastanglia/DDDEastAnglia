using System;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    public class SponsorController : Controller
    {
        private readonly ISponsorRepository sponsorRepository;

        public SponsorController(ISponsorRepository sponsorRepository)
        {
            if (sponsorRepository == null)
            {
                throw new ArgumentNullException("sponsorRepository");
            }

            this.sponsorRepository = sponsorRepository;
        }

        public ActionResult Index()
        {
            var sponsors = sponsorRepository.GetAllSponsors()
                                    .Select(CreateSponsorModel)
                                    .ToList();
            return View(sponsors);
        }

        private SponsorModel CreateSponsorModel(Sponsor sponsor)
        {
            return new SponsorModel
            {
                SponsorId = sponsor.SponsorId,
                Name = sponsor.Name,
                Url = sponsor.Url
            };
        }
    }
}