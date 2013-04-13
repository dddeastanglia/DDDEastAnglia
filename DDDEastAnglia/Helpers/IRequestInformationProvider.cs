using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IRequestInformationProvider
    {
        string GetIPAddress();
        bool IsLoggedIn();
        UserProfile GetCurrentUser();
    }
}