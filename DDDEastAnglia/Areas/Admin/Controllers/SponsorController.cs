using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
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

        [HttpGet]
        public ActionResult Add()
        {
            return View(new SponsorModel());
        }

        [HttpPost]
        public ActionResult Add(SponsorModel sponsor)
        {
            sponsorRepository.Add(Map(sponsor));
            return RedirectToAction("Index");
        }

        private Sponsor Map(SponsorModel sponsorModel)
        {
            return new Sponsor
            {
                Name = sponsorModel.Name,
                Url = sponsorModel.Url,
                SponsorshipAmount = sponsorModel.SponsorshipAmount,
                Logo = GetLogoFromRequest()
            };
        }

        private byte[] GetLogoFromRequest()
        {
            var file = Request.Files["Logo"];
            var ms = new MemoryStream();
            file.InputStream.CopyTo(ms);
            return ms.ToArray();

        }

        private SponsorModel CreateSponsorModel(Sponsor sponsor)
        {
            return new SponsorModel
            {
                SponsorId = sponsor.SponsorId,
                Name = sponsor.Name,
                Url = sponsor.Url,
                SponsorshipAmount = sponsor.SponsorshipAmount
            };
        }
    }
}