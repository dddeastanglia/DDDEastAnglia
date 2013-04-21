using System.Web;

namespace DDDEastAnglia.Helpers.Context
{
    public class HttpWebRequestFactory : IRequestFactory
    {
        public Request GetCurrentRequest()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }
    }
}