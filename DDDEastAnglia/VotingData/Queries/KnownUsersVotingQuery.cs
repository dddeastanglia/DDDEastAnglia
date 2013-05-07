using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class KnownUsersVotingQuery : IQuery<KnownUserVoteCountModel>
    {
        public string Sql
        {
            get {
                return @"
SELECT v.UserId, p.Name, p.UserName, COUNT(v.UserId) AS VoteCount FROM Votes v
JOIN UserProfile p ON v.UserId = p.UserId
WHERE v.UserId <> 0
GROUP BY v.UserId, p.Name, p.Username
ORDER BY VoteCount DESC";
            }
        }

        public IQueryResultObjectFactory<KnownUserVoteCountModel> ObjectFactory
        {
            get
            {
                return new UserVoteCountModelFactory();
            }
        }

        private class UserVoteCountModelFactory : IQueryResultObjectFactory<KnownUserVoteCountModel>
        {
            public KnownUserVoteCountModel Create(IDataReader reader)
            {
                int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string username = reader.GetString(reader.GetOrdinal("UserName"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new KnownUserVoteCountModel
                    {
                        UserId = userId,
                        Name = name ?? username,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}