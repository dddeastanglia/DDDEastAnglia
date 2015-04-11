using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IMessenger<in T>
    {
        void Notify(UserProfile user, T message);
    }
}
