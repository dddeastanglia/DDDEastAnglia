using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        readonly ISponsorRepository sponsorRepository;

        public SponsorController(ISponsorRepository sponsorRepository)
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
            return PartialView("_Sponsors", GetSponsors());
        }

        private IEnumerable<SponsorModel> GetSponsors()
        {
            var allSponsors = sponsorRepository
                .GetAllSponsors()
                .OrderByDescending(s => s.SponsorshipAmount).ThenBy(s => s.PaymentDate)
                .Select(x => new SponsorModel {Name = x.Name, SponsorId = x.SponsorId, Url = x.Url})
                ;
            return allSponsors;
        }
    }
}
