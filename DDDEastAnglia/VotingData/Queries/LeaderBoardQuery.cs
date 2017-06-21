using System;
using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class LeaderBoardQuery : IQuery<SessionLeaderBoardEntry>
    {
        private readonly int limit;
        private readonly bool allowDuplicateSpeakers;

        public LeaderBoardQuery(int limit, bool allowDuplicateSpeakers)
        {
            this.limit = limit;
            this.allowDuplicateSpeakers = allowDuplicateSpeakers;
        }

        public string Sql
        {
            get
            {
                string sql = allowDuplicateSpeakers ? AllowDuplicateSpeakersSql : ForbidDuplicateSpeakersSql;
                return string.Format(sql, limit == int.MaxValue ? "" : "TOP " + limit);
            }
        }

        private const string AllowDuplicateSpeakersSql = @"
SELECT {0} row_number() OVER (ORDER BY COUNT(v.SessionId) DESC) AS Position,
s.SessionId AS SessionId, s.Title AS SessionTitle, u.UserId AS SpeakerUserId,
u.Name AS SpeakerName, COUNT(v.SessionId) AS VoteCount
FROM Sessions s
JOIN Votes v ON v.SessionId = s.SessionId
JOIN UserProfiles u ON u.UserName = s.SpeakerUserName
GROUP BY s.SessionId, s.Title, u.UserId, u.Name
ORDER BY VoteCount DESC";

        private const string ForbidDuplicateSpeakersSql = @"
WITH CTE AS
(
    SELECT row_number() OVER (PARTITION BY u.UserId ORDER BY COUNT(v.SessionId) DESC) AS Position,
    s.SessionId AS SessionId, s.Title AS SessionTitle, u.UserId AS SpeakerUserId,
    u.Name AS SpeakerName, COUNT(v.SessionId) AS VoteCount
    FROM Sessions s
    JOIN Votes v ON v.SessionId = s.SessionId
    JOIN UserProfiles u ON u.UserName = s.SpeakerUserName
    GROUP BY s.SessionId, s.Title, u.UserId, u.Name
)
SELECT {0} row_number() OVER (ORDER BY VoteCount DESC) AS Position,
SessionId, SessionTitle, SpeakerUserId, SpeakerName, VoteCount
FROM CTE
WHERE Position = 1
ORDER BY VoteCount DESC
";

        public IQueryResultObjectFactory<SessionLeaderBoardEntry> ObjectFactory => new LeaderBoardSessionFactory();

        private class LeaderBoardSessionFactory : IQueryResultObjectFactory<SessionLeaderBoardEntry>
        {
            public SessionLeaderBoardEntry Create(IDataReader reader)
            {
                int position = Convert.ToInt32(reader.GetInt64(reader.GetOrdinal("Position")));
                int sessionId = reader.GetInt32(reader.GetOrdinal("SessionId"));
                string sessionTitle = reader.GetString(reader.GetOrdinal("SessionTitle"));
                int speakerUserId = reader.GetInt32(reader.GetOrdinal("SpeakerUserId"));
                string speakerName = reader.GetString(reader.GetOrdinal("SpeakerName"));
                int numberOfVotes = reader.GetInt32(reader.GetOrdinal("VoteCount"));

                return new SessionLeaderBoardEntry
                    {
                        Position = position,
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