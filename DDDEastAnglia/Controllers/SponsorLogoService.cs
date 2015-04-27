using System.Web.Mvc;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Controllers
{
    public class SponsorLogoService
    {
        private readonly ISponsorRepository sponsorRepository;

        public SponsorLogoService(ISponsorRepository sponsorRepository)
        {
            this.sponsorRepository = sponsorRepository;
        }

        public FileContentResult Get(int sponsorId)
        {
            var sponsor = sponsorRepository.GetSponsor(sponsorId);
            return new FileContentResult(sponsor.Logo, "image/png");
        }
    }
}
