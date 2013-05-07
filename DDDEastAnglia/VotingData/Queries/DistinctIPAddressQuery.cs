using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class DistinctIPAddressQuery : IQuery<IPAddressModel>
    {
        public string Sql
        {
            get
            {
                return @"
SELECT IPAddress, COUNT(IPAddress) AS VoteCount
FROM Votes
GROUP BY IPAddress
ORDER BY VoteCount DESC";
            }
        }

        public IQueryResultObjectFactory<IPAddressModel> ObjectFactory
        {
            get
            {
                return new IPAddressModelFactory();
            }
        }

        private class IPAddressModelFactory : IQueryResultObjectFactory<IPAddressModel>
        {
            public IPAddressModel Create(IDataReader reader)
            {
                string ipAddress = reader.GetString(reader.GetOrdinal("IPAddress"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new IPAddressModel
                    {
                        IPAddress = ipAddress,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}