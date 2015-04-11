using System;
using System.IO;
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
                                    .OrderByDescending(s => s.SponsorshipAmount)
                                    .ThenBy(s => s.PaymentDate)
                                    .ToList();
            return View(sponsors);
        }

        public ActionResult Create()
        {
            return View(new SponsorModel());
        }

        [HttpPost]
        public ActionResult Create(SponsorModel sponsor)
        {
            sponsorRepository.AddSponsor(CreateSponsor(sponsor));
            return RedirectToAction("Index");
        }

        public ActionResult Logo(int sponsorId)
        {
            var sponsor = sponsorRepository.GetSponsor(sponsorId);
            return File(sponsor.Logo, "image/png");
        }

        private byte[] GetLogoFromRequest()
        {
            var file = Request.Files["Logo"];

            if (file == null)
            {
                return null;
            }

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
                SponsorshipAmount = sponsor.SponsorshipAmount,
                PaymentDate = sponsor.PaymentDate
            };
        }

        private Sponsor CreateSponsor(SponsorModel sponsorModel)
        {
            return new Sponsor
            {
                Name = sponsorModel.Name,
                Url = sponsorModel.Url,
                SponsorshipAmount = sponsorModel.SponsorshipAmount,
                Logo = GetLogoFromRequest(),
                PaymentDate = sponsorModel.PaymentDate
            };
        }
    }
}