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
        private readonly ISponsorSorter sponsorSorter;

        public SponsorController(ISponsorRepository sponsorRepository, ISponsorSorter sponsorSorter)
        {
            if (sponsorRepository == null)
            {
                throw new ArgumentNullException("sponsorRepository");
            }

            if (sponsorRepository == null)
            {
                throw new ArgumentNullException("sponsorRepository");
            }

            this.sponsorRepository = sponsorRepository;
            this.sponsorSorter = sponsorSorter;
        }

        public ActionResult Index()
        {
            var sponsors = sponsorRepository.GetAllSponsors();
            var sortedSponsors = sponsorSorter.Sort(sponsors)
                                              .Select(CreateSponsorModel)
                                              .ToList();
            return View(sortedSponsors);
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

        public ActionResult Delete(int id)
        {
            var sponsor = sponsorRepository.GetSponsor(id);
            return sponsor == null ? (ActionResult)HttpNotFound() : View(sponsor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sponsorRepository.DeleteSponsor(id);
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
                PaymentDate = sponsor.PaymentDate,
                ShowPublicly = sponsor.ShowPublicly
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
                PaymentDate = sponsorModel.PaymentDate,
                ShowPublicly = sponsorModel.ShowPublicly
            };
        }
    }
}