using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;

namespace DDDEastAnglia.Tests.Controllers
{
    public class HomeControllerBuilder
    {
        private IConferenceLoader conferenceLoader = new ConferenceLoaderBuilder().Build();
        private SponsorModelQuery sponsorModelQuery = new SponsorModelQueryBuilder().Build();

        public HomeControllerBuilder WithConferenceLoader(IConferenceLoader conferenceLoader)
        {
            this.conferenceLoader = conferenceLoader;
            return this;
        }

        public HomeControllerBuilder WithSponsorModelQuery(SponsorModelQuery sponsorModelQuery)
        {
            this.sponsorModelQuery = sponsorModelQuery;
            return this;
        }

        public HomeController Build()
        {
            return new HomeController(conferenceLoader, sponsorModelQuery);
        }
    }
}