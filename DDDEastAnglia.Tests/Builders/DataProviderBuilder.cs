using System.Collections.Generic;
using DDDEastAnglia.VotingData;
using DDDEastAnglia.VotingData.Models;
using NSubstitute;

namespace DDDEastAnglia.Tests.Builders
{
    public class DataProviderBuilder
    {
        private readonly IDataProvider dataProvider;

        public DataProviderBuilder()
        {
            dataProvider = Substitute.For<IDataProvider>();
        }

        public DataProviderBuilder WithNumberOfDaysSinceVotingOpened(int numberOfDays)
        {
            dataProvider.GetNumberOfDaysSinceVotingOpened().Returns(numberOfDays);
            return this;
        }

        public DataProviderBuilder WithNumberOfDaysOfVoting(int numberOfDays)
        {
            dataProvider.GetNumberOfDaysOfVoting().Returns(numberOfDays);
            return this;
        }

        public DataProviderBuilder WithLeaderboard(IList<SessionLeaderBoardEntry> sessions)
        {
            dataProvider.GetLeaderBoard(Arg.Any<int>(), Arg.Any<bool>()).Returns(sessions);
            return this;
        }

        public DataProviderBuilder WithVotesForDistinctIPAddresses(VotesForIPAddressModel[] votes)
        {
            dataProvider.GetDistinctIPAddresses().Returns(votes);
            return this;
        }

        public DataProviderBuilder WithVotesForIPAddresses(CookieVoteModel[] votes)
        {
            dataProvider.GetVotesPerIPAddress(Arg.Any<string>()).Returns(votes);
            return this;
        }
            
        public DataProviderBuilder WithVotesPerDate(DateTimeVoteModel[] votes)
        {
            dataProvider.GetVotesPerDate().Returns(votes);
            return this;
        }

        public DataProviderBuilder WithVotesPerDay(DayOfWeekVoteModel[] votes)
        {
            dataProvider.GetVotesPerDay().Returns(votes);
            return this;
        }

        public DataProviderBuilder WithVotesPerHour(DateTimeVoteModel[] votes)
        {
            dataProvider.GetVotesPerHour().Returns(votes);
            return this;
        }

        public DataProviderBuilder WithVotersForIPAddresses(IPAddressVoterModel[] voters)
        {
            dataProvider.GetVotersPerIPAddress().Returns(voters);
            return this;
        }

        public DataProviderBuilder WithNumberOfVotesCastCounts(NumberOfUsersWithVotesModel[] voteCounts)
        {
            dataProvider.GetNumberOfVotesCastCounts().Returns(voteCounts);
            return this;
        }

        public IDataProvider Build()
        {
            return dataProvider;
        }
    }
}
