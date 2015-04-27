using System.Collections.Generic;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.Tests
{
    internal class InMemorySponsorRepository : InMemoryRepository<Sponsor>, ISponsorRepository
    {
        public InMemorySponsorRepository() : base(s => s.SponsorId)
        {
        }

        public IEnumerable<Sponsor> GetAllSponsors()
        {
            return GetAll();
        }

        public void AddSponsor(Sponsor sponsor)
        {
            Save(sponsor);
        }

        public void DeleteSponsor(int sponsorId)
        {
            Delete(sponsorId);
        }

        public Sponsor GetSponsor(int sponsorId)
        {
            return Get(sponsorId);
        }
    }
}