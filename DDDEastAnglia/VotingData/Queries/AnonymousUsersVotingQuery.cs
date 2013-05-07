using System;
using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class AnonymousUsersVotingQuery : IQuery<AnonymousUserVoteCountModel>
    {
        public string Sql
        {
            get {
                return @"
SELECT CookieId, COUNT(CookieId) AS VoteCount 
FROM Votes
WHERE UserId = 0
GROUP BY CookieId
ORDER BY VoteCount DESC
";
            }
        }

        public IQueryResultObjectFactory<AnonymousUserVoteCountModel> ObjectFactory
        {
            get
            {
                return new UserVoteCountModelFactory();
            }
        }

        private class UserVoteCountModelFactory : IQueryResultObjectFactory<AnonymousUserVoteCountModel>
        {
            public AnonymousUserVoteCountModel Create(IDataReader reader)
            {
                Guid cookieId = reader.GetGuid(reader.GetOrdinal("CookieId"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new AnonymousUserVoteCountModel
                    {
                        CookieId = cookieId,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}