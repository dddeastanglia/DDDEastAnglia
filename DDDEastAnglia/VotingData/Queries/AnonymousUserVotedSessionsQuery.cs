using System;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class AnonymousUserVotedSessionsQuery : IQuery<VotedSessionModel>
    {
        private readonly Guid cookieId;

        public AnonymousUserVotedSessionsQuery(Guid cookieId)
        {
            this.cookieId = cookieId;
        }

        public string Sql => $@"
SELECT s.SessionId AS SessionId, s.Title AS SessionTitle, p.UserId AS SpeakerUserId, p.Name AS SpeakerName
FROM Votes v, Sessions s, UserProfiles p
WHERE v.SessionId = s.SessionId
AND s.SpeakerUserName = p.UserName
AND v.CookieId = '{cookieId}'
AND v.UserId IS NULL
ORDER BY SpeakerName";

        public IQueryResultObjectFactory<VotedSessionModel> ObjectFactory => new VotedSessionModelFactory();
    }
}
