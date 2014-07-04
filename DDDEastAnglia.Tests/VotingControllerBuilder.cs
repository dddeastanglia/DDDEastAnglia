using DDDEastAnglia.Areas.Admin.Controllers;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData;
using NSubstitute;

namespace DDDEastAnglia.Tests
{
    public class VotingControllerBuilder
    {
        private IDataProvider dataProvider;
        private IDnsLookup dnsLookup;
        private IChartDataConverter chartDataConverter;

        public VotingControllerBuilder()
        {
            dataProvider = Substitute.For<IDataProvider>();
            dnsLookup = Substitute.For<IDnsLookup>();
            chartDataConverter = Substitute.For<IChartDataConverter>();
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
            var conferenceLoader = Substitute.For<IConferenceLoader>();
            return new VotingController(conferenceLoader, dataProvider, dnsLookup, chartDataConverter);
        }
    }
}
