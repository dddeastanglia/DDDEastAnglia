using System.Data;
using DDDEastAnglia.VotingData.Models;

namespace DDDEastAnglia.VotingData.Queries
{
    public class VotedSessionModelFactory : IQueryResultObjectFactory<VotedSessionModel>
    {
        public VotedSessionModel Create(IDataReader reader)
        {
            int sessionId = reader.GetInt32(reader.GetOrdinal("SessionId"));
            string sessionTitle = reader.GetString(reader.GetOrdinal("SessionTitle"));
            int speakerUserId = reader.GetInt32(reader.GetOrdinal("SpeakerUserId"));
            string speakerName = reader.GetString(reader.GetOrdinal("SpeakerName"));

            return new VotedSessionModel
                {
                            SessionId = sessionId,
                            SessionTitle = sessionTitle,
                            SpeakerUserId = speakerUserId,
                            SpeakerName = speakerName
                };
        }
    }
}