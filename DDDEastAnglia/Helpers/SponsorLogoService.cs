using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface ISponsorLogoService
    {
        SponsorLogo Get(int sponsorId);
    }

    public class SponsorLogoService : ISponsorLogoService
    {
        private readonly ISponsorRepository sponsorRepository;

        public SponsorLogoService(ISponsorRepository sponsorRepository)
        {
            if (sponsorRepository == null)
            {
                throw new ArgumentNullException("sponsorRepository");
            }

            this.sponsorRepository = sponsorRepository;
        }

        public SponsorLogo Get(int sponsorId)
        {
            var sponsor = sponsorRepository.GetSponsor(sponsorId);
            return new SponsorLogo(sponsor.Logo, "image/png");
        }
    }
}
