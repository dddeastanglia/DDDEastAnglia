using System.Collections.Generic;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class VotersPerIPAddressViewModel
    {
        public IList<IPAddressVoterModel> IPAddressVoters{get;set;}
    }
}