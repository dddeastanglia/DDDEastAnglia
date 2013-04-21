using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public interface ISessionRepository
    {
        Session Get(int id);
        bool Exists(int id);
    }
}