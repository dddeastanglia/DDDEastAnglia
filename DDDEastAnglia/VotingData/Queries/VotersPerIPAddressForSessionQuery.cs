using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class VotersPerIPAddressForSessionQuery : IQuery<VotersPerIPAddressForSessionModel>
    {
        private readonly int sessionId;

        public VotersPerIPAddressForSessionQuery(int sessionId)
        {
            this.sessionId = sessionId;
        }

        public string Sql
        {
            get
            {
                return string.Format(@"SELECT V.IPAddress, COUNT(V.IPAddress) VoteCount
FROM Votes V 
WHERE V.sessionid = {0}
GROUP BY IPAddress
ORDER BY VoteCount DESC", 
                    sessionId);
            }
        }

        public IQueryResultObjectFactory<VotersPerIPAddressForSessionModel> ObjectFactory
        {
            get
            {
                return new VotersPerIPAddressForSessionModelFactory();
            }
        }

        private class VotersPerIPAddressForSessionModelFactory : IQueryResultObjectFactory<VotersPerIPAddressForSessionModel>
        {
            public VotersPerIPAddressForSessionModel Create(IDataReader reader)
            {
                string ipAddress = reader.GetString(reader.GetOrdinal("IPAddress"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new VotersPerIPAddressForSessionModel
                {
                    IPAddress = ipAddress,
                    NumberOfVotes = numberOfVotes
                };
            }
        }
    }
}
