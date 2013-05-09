using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class KnownUserVotedSessionsQuery : IQuery<VotedSessionModel>
    {
        private readonly int userId;

        public KnownUserVotedSessionsQuery(int userId)
        {
            this.userId = userId;
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
AND v.UserId = {0}
ORDER BY SpeakerName",
                   userId);
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
