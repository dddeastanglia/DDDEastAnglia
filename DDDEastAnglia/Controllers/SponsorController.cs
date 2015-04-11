using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SponsorController : Controller
    {
        readonly ISponsorRepository sponsorRepository;
        readonly ISponsorSorter sponsorSorter;

        public SponsorController(ISponsorRepository sponsorRepository, ISponsorSorter sponsorSorter)
        {
            this.sponsorRepository = sponsorRepository;
            this.sponsorSorter = sponsorSorter;
        }

        public ActionResult Logo(int sponsorId)
        {
            var sponsor = sponsorRepository.GetSponsor(sponsorId);
            return File(sponsor.Logo, "image/png");
        }

        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            return PartialView("_Sponsors", sponsorSorter.Sort(sponsorRepository.GetAllSponsors()).Select(x => new SponsorModel { Name = x.Name, SponsorId = x.SponsorId, Url = x.Url }));
        }
    }
}
