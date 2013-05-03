using System.Collections.Generic;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class IPAddressStatsViewModel
    {
        public int HighestVoteCount{get;set;}
        public IList<IPAddressModel> IPAddresses{get;set;}         
    }
}