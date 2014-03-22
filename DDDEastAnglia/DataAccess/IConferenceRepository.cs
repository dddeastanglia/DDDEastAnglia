using DDDEastAnglia.DataAccess.EntityFramework.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface IConferenceRepository
    {
        Conference ForSession(int sessionId);
        Conference GetByEventShortName(string shortName);
    }
}
