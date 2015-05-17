using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData;
using NSubstitute;

namespace DDDEastAnglia.Tests.Builders
{
    public class VotingControllerBuilder
    {
        private IConferenceLoader conferenceLoader;
        private IDataProvider dataProvider;
        private IDnsLookup dnsLookup;
        private IChartDataConverter chartDataConverter;

        public VotingControllerBuilder()
        {
            conferenceLoader = Substitute.For<IConferenceLoader>();
            dataProvider = Substitute.For<IDataProvider>();
            dnsLookup = Substitute.For<IDnsLookup>();
            chartDataConverter = Substitute.For<IChartDataConverter>();
        }

        public VotingControllerBuilder WithConferenceLoader(IConferenceLoader conferenceLoader)
        {
            this.conferenceLoader = conferenceLoader;
            return this;
        }

        public VotingControllerBuilder WithDataProvider(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            return this;
        }

        public VotingControllerBuilder WithDnsLookup(IDnsLookup dnsLookup)
        {
            this.dnsLookup = dnsLookup;
            return this;
        }

        public VotingControllerBuilder WithChartDataConverter(IChartDataConverter chartDataConverter)
        {
            this.chartDataConverter = chartDataConverter;
            return this;
        }

        public VotingController Build()
        {
            return new VotingController(conferenceLoader, dataProvider, dnsLookup, chartDataConverter);
        }
    }
}
