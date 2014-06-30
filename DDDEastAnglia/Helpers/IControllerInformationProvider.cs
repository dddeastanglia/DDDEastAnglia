using System;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IControllerInformationProvider
    {
        string UserAgent { get; }
        string Referrer { get; }
        string SessionId { get; }
        bool IsAjaxRequest { get; }
        string GetIPAddress();
        bool IsLoggedIn();
        UserProfile GetCurrentUser();
        DateTime UtcNow { get; }
        VotingCookie GetVotingCookie();
        void SaveVotingCookie(VotingCookie cookie);
    }
}
