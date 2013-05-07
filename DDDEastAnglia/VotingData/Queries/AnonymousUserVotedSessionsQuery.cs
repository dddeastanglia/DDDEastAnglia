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

        public string Sql
        {
            get
            {
                return string.Format(@"
SELECT s.SessionId AS SessionId, s.Title AS SessionTitle, p.UserId AS SpeakerUserId, p.Name AS SpeakerName
FROM Votes v, Sessions s, UserProfile p
WHERE v.SessionId = s.SessionId
AND s.SpeakerUserName = p.UserName
AND v.CookieId = '{0}'
AND v.UserId = 0
ORDER BY SpeakerName",
                   cookieId);
            }
        }

        public IQueryResultObjectFactory<VotedSessionModel> ObjectFactory
        {
            get
            {
                return new VotedSessionModelFactory();
            }
        }
    }
}
