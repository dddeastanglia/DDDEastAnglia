using System;
using System.Data;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class KnownUsersVotingQuery : IQuery<KnownUserVoteCountModel>
    {
        private readonly GravatarUrl gravatar;

        public KnownUsersVotingQuery(GravatarUrl gravatar)
        {
            if (gravatar == null)
            {
                throw new ArgumentNullException("gravatar");
            }

            this.gravatar = gravatar;
        }

        public string Sql => @"
SELECT v.UserId, p.Name, p.UserName, p.EmailAddress, COUNT(v.UserId) AS VoteCount FROM Votes v
JOIN UserProfiles p ON v.UserId = p.UserId
WHERE v.UserId <> 0
GROUP BY v.UserId, p.Name, p.Username, p.EmailAddress
ORDER BY VoteCount DESC";

        public IQueryResultObjectFactory<KnownUserVoteCountModel> ObjectFactory => new UserVoteCountModelFactory(gravatar);

        private class UserVoteCountModelFactory : IQueryResultObjectFactory<KnownUserVoteCountModel>
        {
            private readonly GravatarUrl gravatar;

            public UserVoteCountModelFactory(GravatarUrl gravatar)
            {
                if (gravatar == null)
                {
                    throw new ArgumentNullException("gravatar");
                }

                this.gravatar = gravatar;
            }

            public KnownUserVoteCountModel Create(IDataReader reader)
            {
                int userId = reader.GetInt32(reader.GetOrdinal("UserId"));
                string name = reader.GetString(reader.GetOrdinal("Name"));
                string emailAddress = reader.GetString(reader.GetOrdinal("EmailAddress"));
                string username = reader.GetString(reader.GetOrdinal("UserName"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new KnownUserVoteCountModel
                    {
                        UserId = userId,
                        GravatarUrl = gravatar.GetUrl(emailAddress),
                        Name = string.IsNullOrWhiteSpace(name) ? username : name,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}