using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData.Models;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SponsorModelQuery
    {
        private readonly ISponsorRepository sponsorRepository;

        public SponsorModelQuery(ISponsorRepository sponsorRepository)
        {
            this.sponsorRepository = sponsorRepository;
        }

        public IEnumerable<SponsorModel> Get()
        {
            var sponsors =
                sponsorRepository
                    .GetAllSponsors()
                    .Where(x => x.ShowPublicly)
                    .OrderBySponsorSorter();

            return sponsors.Select(ToViewModel);
        }

        private SponsorModel ToViewModel(Sponsor sponsor)
        {
            return new SponsorModel
            {
                Name = sponsor.Name, 
                SponsorId = sponsor.SponsorId,
                Url = sponsor.Url
            };
        }
    }
    
}