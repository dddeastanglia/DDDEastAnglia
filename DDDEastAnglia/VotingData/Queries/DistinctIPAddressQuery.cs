using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class DistinctIPAddressQuery : IQuery<VotesForIPAddressModel>
    {
        public string Sql => @"
SELECT IPAddress, COUNT(IPAddress) AS VoteCount
FROM Votes
GROUP BY IPAddress
ORDER BY VoteCount DESC";

        public IQueryResultObjectFactory<VotesForIPAddressModel> ObjectFactory => new IPAddressModelFactory();

        private class IPAddressModelFactory : IQueryResultObjectFactory<VotesForIPAddressModel>
        {
            public VotesForIPAddressModel Create(IDataReader reader)
            {
                string ipAddress = reader.GetString(reader.GetOrdinal("IPAddress"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new VotesForIPAddressModel
                    {
                        IPAddress = ipAddress,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}