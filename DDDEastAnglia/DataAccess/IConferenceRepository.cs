using DDDEastAnglia.DataAccess.SimpleData.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IConferenceRepository
    {
        Conference ForSession(int sessionId);
        Conference GetByEventShortName(string shortName);
    }
}
