using System;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Helpers
{
    public interface IRequestFactory
    {
        Request GetCurrentRequest();
    }

    public abstract class Request
    {
        public abstract string UserAgent { get; }
        public abstract string Referrer { get; }
        public abstract string SessionId { get; }
        public abstract bool IsAjaxRequest { get; }
        public abstract string GetIPAddress();
        public abstract bool IsLoggedIn();
        public abstract UserProfile GetCurrentUser();
        public abstract DateTime UtcNow { get; }
    }
}