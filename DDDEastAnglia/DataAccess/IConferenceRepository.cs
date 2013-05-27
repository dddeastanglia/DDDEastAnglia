using DDDEastAnglia.Domain;

namespace DDDEastAnglia.DataAccess
{
    public interface IConferenceRepository
    {
        IConference ForSession(int sessionId);
        IConference GetByEventShortName(string shortName);
    }
}