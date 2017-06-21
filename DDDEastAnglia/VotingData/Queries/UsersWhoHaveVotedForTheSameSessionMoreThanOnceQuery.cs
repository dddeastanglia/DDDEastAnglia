using System;
using System.Data;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class UsersWhoHaveVotedForTheSameSessionMoreThanOnceQuery : IQuery<DuplicateVoteModel>
    {
        private readonly GravatarUrl gravatarUrl;

        public UsersWhoHaveVotedForTheSameSessionMoreThanOnceQuery(GravatarUrl gravatarUrl)
        {
            if (gravatarUrl == null)
            {
                throw new ArgumentNullException(nameof(gravatarUrl));
            }

            this.gravatarUrl = gravatarUrl;
        }

        public string Sql => @"
SELECT CookieId, V.UserId, P.EmailAddress, V.SessionId, S.Title AS SessionTitle, SP.UserId AS SpeakerUserId, SP.Name AS SpeakerName, COUNT(CookieId) AS VoteCount 
FROM Sessions S, UserProfiles SP, Votes V LEFT OUTER JOIN UserProfiles P
ON V.UserId = P.UserId
WHERE S.SessionId = V.SessionId
AND S.SpeakerUserName = SP.UserName
GROUP BY CookieId, V.UserId, P.EmailAddress, V.SessionId, S.Title, SP.UserId, SP.Name
HAVING COUNT(CookieId) > 1
ORDER BY VoteCount DESC";

        public IQueryResultObjectFactory<DuplicateVoteModel> ObjectFactory => new DuplicateVoteModelFactory(gravatarUrl);

        private class DuplicateVoteModelFactory : IQueryResultObjectFactory<DuplicateVoteModel>
        {
            private readonly GravatarUrl gravatarUrl;

            public DuplicateVoteModelFactory(GravatarUrl gravatarUrl)
            {
                if (gravatarUrl == null)
                {
                    throw new ArgumentNullException(nameof(gravatarUrl));
                }

                this.gravatarUrl = gravatarUrl;
            }

            public DuplicateVoteModel Create(IDataReader reader)
            {
                Guid cookieId = reader.GetGuid(reader.GetOrdinal("CookieId"));
                int userIdField = reader.GetOrdinal("UserId");
                int? userId = reader.IsDBNull(userIdField) ? (int?) null : reader.GetInt32(userIdField);
                int emailAddressOrdinal = reader.GetOrdinal("EmailAddress");
                string emailAddress = reader.IsDBNull(emailAddressOrdinal) ? null : reader.GetString(emailAddressOrdinal);

                int sessionId = reader.GetInt32(reader.GetOrdinal("SessionId"));
                string sessionTitle = reader.GetString(reader.GetOrdinal("SessionTitle"));

                int speakerUserId = reader.GetInt32(reader.GetOrdinal("SpeakerUserId"));
                string speakerName = reader.GetString(reader.GetOrdinal("SpeakerName"));

                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                bool userIsUnknown = !userId.HasValue;
                string gravatar = gravatarUrl.GetUrl(userIsUnknown ? cookieId.ToString() : emailAddress, useIdenticon: userIsUnknown);

                return new DuplicateVoteModel
                    {
                        CookieId = cookieId,
                        UserId = userId,
                        GravatarUrl = gravatar,
                        SessionId = sessionId,
                        SessionTitle = sessionTitle,
                        SpeakerUserId = speakerUserId,
                        SpeakerName = speakerName,
                        NumberOfVotes = numberOfVotes
                    };
            }
        }
    }
}