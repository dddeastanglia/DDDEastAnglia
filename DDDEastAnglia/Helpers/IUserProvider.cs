using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IUserProvider
    {
        bool IsLoggedIn();
        UserProfile GetCurrentUser();
    }
}