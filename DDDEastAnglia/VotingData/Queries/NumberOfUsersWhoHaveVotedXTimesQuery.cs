using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class NumberOfUsersWhoHaveVotedXTimesQuery : IQuery<NumberOfUsersWithVotesModel>
    {
        public string Sql
        {
            get
            {
                return @"
WITH CTE AS
(
	SELECT COUNT(v.CookieId) AS VoteCount 
	FROM Votes v
	GROUP BY v.CookieId
)
SELECT VoteCount, COUNT(VoteCount) AS NumberOfUsers
FROM CTE
GROUP BY VoteCount
ORDER BY VoteCount ASC";
            }
        }

        public IQueryResultObjectFactory<NumberOfUsersWithVotesModel> ObjectFactory
        {
            get
            {
                return new NumberOfUsersWithVotesModelFactory();
            }
        }

        private class NumberOfUsersWithVotesModelFactory : IQueryResultObjectFactory<NumberOfUsersWithVotesModel>
        {
            public NumberOfUsersWithVotesModel Create(IDataReader reader)
            {
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));
                int numberOfUsers = reader.GetInt32(reader.GetOrdinal("NumberOfUsers"));

                return new NumberOfUsersWithVotesModel
                    {
                            NumberOfVotes = numberOfVotes,
                            NumberOfUsers = numberOfUsers
                    };
            }
        }
    }
}