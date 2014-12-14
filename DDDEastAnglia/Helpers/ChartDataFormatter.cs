using System;
using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IChartDataConverter
    {
        long[][] ToChartData(IList<DayOfWeekVoteModel> voteData);
        long[][] ToChartData(IList<DateTimeVoteModel> voteData, Func<DateTimeVoteModel, long> xAxisValueSelector);
        long[][] ToChartData(IList<NumberOfUsersWithVotesModel> voteData);
    }

    public class ChartDataConverter : IChartDataConverter
    {
        public long[][] ToChartData(IList<DayOfWeekVoteModel> voteData)
        {
            long[][] chartData = new long[voteData.Count][];

            foreach (var item in voteData.Select((v, i) => new { Index = i, Vote = v }))
            {
                var vote = item.Vote;
                chartData[item.Index] = new[] {(long) vote.Day, vote.VoteCount};
            }

            return chartData;
        }

        public long[][] ToChartData(IList<DateTimeVoteModel> voteData, Func<DateTimeVoteModel, long> xAxisValueSelector)
        {
            long[][] chartData = new long[voteData.Count][];

            foreach (var item in voteData.Select((v, i) => new {Index = i, Vote = v}))
            {
                var vote = item.Vote;
                var xAxisValue = xAxisValueSelector(vote);
                chartData[item.Index] = new[] {xAxisValue, vote.VoteCount};
            }

            return chartData;
        }

        public long[][] ToChartData(IList<DateTimeVoteModel> voteData)
        {
            long[][] chartData = new long[voteData.Count][];

            foreach (var item in voteData.Select((v, i) => new {Index = i, Vote = v}))
            {
                var vote = item.Vote;
                chartData[item.Index] = new[] {(long) vote.Date.Hour, vote.VoteCount};
            }

            return chartData;
        }

        public long[][] ToChartData(IList<NumberOfUsersWithVotesModel> voteData)
        {
            long[][] chartData = new long[voteData.Count][];

            foreach (var item in voteData.Select((v, i) => new {Index = i, Vote = v}))
            {
                var vote = item.Vote;
                chartData[item.Index] = new[] {(long) vote.NumberOfVotes, vote.NumberOfUsers};
            }

            return chartData;
        }
    }
}