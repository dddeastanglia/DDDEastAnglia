using System.Collections.Generic;
using System.Linq;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IChartDataConverter
    {
        long[][] ToChartData(IList<DateTimeVoteModel> voteData);
    }

    public class ChartDataConverter : IChartDataConverter
    {
        public long[][] ToChartData(IList<DateTimeVoteModel> voteData)
        {
            long[][] chartData = new long[voteData.Count][];

            foreach (var item in voteData.Select((v, i) => new {Index = i, Vote = v}))
            {
                var vote = item.Vote;
                long javascriptTimestamp = vote.Date.GetJavascriptTimestamp();
                chartData[item.Index] = new[] {javascriptTimestamp, vote.VoteCount};
            }

            return chartData;
        }
    }
}