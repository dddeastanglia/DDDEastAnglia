using DDDEastAnglia.Domain;

namespace DDDEastAnglia.DataAccess
{
    public interface IConferenceRepository
    {
        Conference ForSession(int sessionId);
    }
}