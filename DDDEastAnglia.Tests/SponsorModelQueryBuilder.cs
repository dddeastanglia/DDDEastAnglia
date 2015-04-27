using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Tests
{
    public class SponsorModelQueryBuilder
    {
        private ISponsorRepository sponsorRepository = new InMemorySponsorRepository();

        public SponsorModelQuery Build()
        {
            return new SponsorModelQuery(sponsorRepository);
        }
    }
}