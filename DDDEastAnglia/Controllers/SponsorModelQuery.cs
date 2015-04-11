using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Controllers
{
    public class SponsorModelQuery
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly SponsorSorter _sponsorSorter;

        public SponsorModelQuery(ISponsorRepository sponsorRepository, SponsorSorter sponsorSorter)
        {
            _sponsorRepository = sponsorRepository;
            _sponsorSorter = sponsorSorter;
        }

        public IEnumerable<SponsorModel> Get()
        {
            var filteredSponsors = _sponsorRepository.GetAllSponsors()
                .Where(x => x.ShowPublicly);

            var sortedSponsors = _sponsorSorter.Sort(filteredSponsors);

            return sortedSponsors.Select(x => new SponsorModel {Name = x.Name, SponsorId = x.SponsorId, Url = x.Url});
        }
    }
}