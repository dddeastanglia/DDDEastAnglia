using System;
using System.Web;

namespace DDDEastAnglia.Helpers
{
    public static class CookieExtensions
    {
         public static Guid GetId(this HttpCookie cookie)
         {
             Guid id;
             if (Guid.TryParse(cookie.Value, out id))
             {
                 return id;
             }
             return Guid.Empty;
         }
    }
}