using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Controllers
{
    public class AllPublicSponsors : IViewModelQuery<IEnumerable<SponsorModel>>
    {
        private readonly ISponsorRepository sponsorRepository;
        private readonly ISponsorSorter sponsorSorter;

        public AllPublicSponsors(ISponsorRepository sponsorRepository, ISponsorSorter sponsorSorter)
        {
            if (sponsorRepository == null)
            {
                throw new ArgumentNullException("sponsorRepository");
            }

            if (sponsorSorter == null)
            {
                throw new ArgumentNullException("sponsorSorter");
            }

            this.sponsorRepository = sponsorRepository;
            this.sponsorSorter = sponsorSorter;
        }

        public IEnumerable<SponsorModel> Get()
        {
            var publicSponsors = sponsorRepository
                                    .GetAllSponsors()
                                    .Where(x => x.ShowPublicly);

            return sponsorSorter
                    .Sort(publicSponsors)
                    .Select(ToViewModel);
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
