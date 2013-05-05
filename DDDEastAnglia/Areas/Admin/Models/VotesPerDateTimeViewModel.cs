using System.Collections.Generic;
using System.Text;
using DDDEastAnglia.Helpers;
using System.Linq;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class VotesPerDateTimeViewModel
    {
        public IList<DateTimeVoteModel> Votes{get;set;}

        public string GetChartData()
        {
            var output = new StringBuilder();
            output.Append("[");

            foreach (var item in Votes.Select((v, i) => new {Index = i, Vote = v}))
            {
                output.Append("[");
                output.Append(item.Vote.Date.GetJavascriptTimestamp());
                output.Append(", ");
                output.Append(item.Vote.VoteCount);
                output.Append("]");

                if (item.Index < Votes.Count - 1)
                {
                    output.Append(", ");
                }
            }
             
            output.Append("]");
            return output.ToString();
        }
    }
}