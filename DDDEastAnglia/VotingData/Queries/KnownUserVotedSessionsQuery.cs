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

        public string Sql => $@"
SELECT s.SessionId AS SessionId, s.Title AS SessionTitle, p.UserId AS SpeakerUserId, p.Name AS SpeakerName
FROM Votes v, Sessions s, UserProfiles p
WHERE v.SessionId = s.SessionId
AND s.SpeakerUserName = p.UserName
AND v.UserId = {userId}
ORDER BY SpeakerName";

        public IQueryResultObjectFactory<VotedSessionModel> ObjectFactory => new VotedSessionModelFactory();
    }
}
