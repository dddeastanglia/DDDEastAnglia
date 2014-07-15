using System.Collections.Generic;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData.Models;
using NSubstitute;

namespace DDDEastAnglia.Tests
{
    public class ChartDataConverterBuilder
    {
        private readonly IChartDataConverter chartDataConverter;

        public ChartDataConverterBuilder()
        {
            chartDataConverter = Substitute.For<IChartDataConverter>();
        }

        public ChartDataConverterBuilder WithChartDataPerHour(long[][] chartData)
        {
            chartDataConverter.ToChartData(Arg.Any<IList<DateTimeVoteModel>>()).Returns(chartData);
            return this;
        }

        public ChartDataConverterBuilder WithChartDataPerDay(long[][] chartData)
        {
            chartDataConverter.ToChartData(Arg.Any<IList<DateTimeVoteModel>>()).Returns(chartData);
            return this;
        }

        public ChartDataConverterBuilder WithChartDataPerUser(long[][] chartData)
        {
            chartDataConverter.ToChartData(Arg.Any<IList<NumberOfUsersWithVotesModel>>()).Returns(chartData);
            return this;
        }

        public IChartDataConverter Build()
        {
            return chartDataConverter;
        }
    }
}
