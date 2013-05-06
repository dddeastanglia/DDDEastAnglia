using System.Collections.Generic;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class VotersPerIPAddressViewModel
    {
        public int HighestNumberOfVoters{get;set;}
        public IList<IPAddressVoterModel> IPAddressVoters{get;set;}
    }
}