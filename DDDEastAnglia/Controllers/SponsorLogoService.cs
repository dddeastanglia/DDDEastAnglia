using System;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Controllers
{
    public class SponsorLogoService
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

        public Image Get(int sponsorId)
        {
            var sponsor = sponsorRepository.GetSponsor(sponsorId);
            return new Image(sponsor.Logo, "image/png");
        }

        public class Image
        {
            public byte[] Data { get; private set; }
            public string ContentType { get; private set; }

            public Image(byte[] data, string contentType)
            {
                Data = data;
                ContentType = contentType;
            }
        }
    }
}
