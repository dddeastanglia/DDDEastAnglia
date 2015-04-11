using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger
{
    public interface IMessenger<in T>
    {
        void Notify(UserProfile user, T message);
    }
}
