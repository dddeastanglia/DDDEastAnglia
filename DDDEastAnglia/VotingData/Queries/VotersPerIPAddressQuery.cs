using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class VotersPerIPAddressQuery : IQuery<IPAddressVoterModel>
    {
        public string Sql
        {
            get 
            {
                return @"
SELECT CookieVsIPAddress.IPAddress, COUNT(CookieVsIPAddress.CookieId) AS VoterCount FROM
(
    SELECT IPAddress, CookieId FROM Votes
    GROUP BY IPAddress, CookieId
) AS CookieVsIPAddress
GROUP BY CookieVsIPAddress.IPAddress
ORDER BY VoterCount DESC";            
            }
        }

        public IQueryResultObjectFactory<IPAddressVoterModel> ObjectFactory
        {
            get
            {
                return new IPAddressVoterModelFactory();
            }
        }

        private class IPAddressVoterModelFactory : IQueryResultObjectFactory<IPAddressVoterModel>
        {
            public IPAddressVoterModel Create(IDataReader reader)
            {
                string ipAddress = reader.GetString(reader.GetOrdinal("IPAddress"));
                int numberOfVoters = reader.GetInt32(reader.GetOrdinal("VoterCount"));

                return new IPAddressVoterModel
                    {
                        IPAddress = ipAddress,
                        NumberOfVoters = numberOfVoters
                    };
            }
        }
    }
}
