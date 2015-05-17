using DDDEastAnglia.Models;

namespace DDDEastAnglia.Services.Messenger
{
    public interface IMessenger
    {
        void Notify(UserProfile user);
    }
}
