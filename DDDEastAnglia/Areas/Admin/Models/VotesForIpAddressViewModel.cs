using System.Collections.Generic;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class VotesForIpAddressViewModel
    {
        public string IPAddress{get;set;}
        public int HighestNumberOfVotes{get;set;}
        public IList<CookieVoteModel> DistinctVotes{get;set;}
    }
}