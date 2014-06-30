using System;
using System.Web;
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
        HttpCookie GetCookie();
        void SaveCookie(HttpCookie httpCookie);
    }
}