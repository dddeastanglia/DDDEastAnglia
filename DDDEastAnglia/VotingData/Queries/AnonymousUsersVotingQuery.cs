using System;
using System.Data;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class AnonymousUsersVotingQuery : IQuery<AnonymousUserVoteCountModel>
    {
        private readonly GravatarUrl gravatar;

        public AnonymousUsersVotingQuery(GravatarUrl gravatar)
        {
            if (gravatar == null)
            {
                throw new ArgumentNullException("gravatar");
            }
            
            this.gravatar = gravatar;
        }

        public string Sql
        {
            get 
            {
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
                return new UserVoteCountModelFactory(gravatar);
            }
        }

        private class UserVoteCountModelFactory : IQueryResultObjectFactory<AnonymousUserVoteCountModel>
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

            public AnonymousUserVoteCountModel Create(IDataReader reader)
            {
                Guid cookieId = reader.GetGuid(reader.GetOrdinal("CookieId"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new AnonymousUserVoteCountModel
                    {
                        CookieId = cookieId,
                        GravatarUrl = gravatar.GetUrl(cookieId.ToString(), useIdenticon: true),
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}