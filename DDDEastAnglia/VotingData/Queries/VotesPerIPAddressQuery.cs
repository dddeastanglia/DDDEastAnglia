using System;
using System.Data;
using System.Net;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class VotesPerIPAddressQuery : IQuery<CookieVoteModel>
    {
        private readonly string ipAddress;

        public VotesPerIPAddressQuery(string ipAddress)
        {
            IPAddress.Parse(ipAddress);
            this.ipAddress = ipAddress;
        }

        public string Sql
        {
            get
            {
                const string sql = @"
SELECT CookieId, COUNT(CookieId) AS VoteCount FROM Votes WHERE IPAddress LIKE '{0}'
GROUP BY CookieId
ORDER BY COUNT(CookieId) DESC
";
                return string.Format(sql, ipAddress);
            }
        }

        public IQueryResultObjectFactory<CookieVoteModel> ObjectFactory
        {
            get
            {
                return new CookieVoteModelFactory();
            }
        }

        private class CookieVoteModelFactory : IQueryResultObjectFactory<CookieVoteModel>
        {
            public CookieVoteModel Create(IDataReader reader)
            {
                Guid cookieId = reader.GetGuid(reader.GetOrdinal("CookieId"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new CookieVoteModel
                    {
                        CookieId = cookieId,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}