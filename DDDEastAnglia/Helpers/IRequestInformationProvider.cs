using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IRequestInformationProvider
    {
        string UserAgent { get; }
        string Referrer { get; }
        string GetIPAddress();
        bool IsLoggedIn();
        UserProfile GetCurrentUser();
    }
}