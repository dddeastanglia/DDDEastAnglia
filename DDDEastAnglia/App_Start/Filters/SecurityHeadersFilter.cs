using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDDEastAnglia.Filters
{
    public class SecurityHeadersFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            HttpContextBase contextBase = filterContext.HttpContext;
            HttpResponseBase responseBase = contextBase.Response;
            NameValueCollection headers = responseBase.Headers;

            RemoveASPNETHeaders(headers);

            AddSecurityHeaders(headers);
        }

        private void AddSecurityHeaders(NameValueCollection headers)
        {
            AddHeader(headers, "X-Frame-Origins", "SAMEORIGIN");
            AddHeader(headers, "X-XSS-Protection", "1; mode=block");
            AddHeader(headers, "X-Content-Type-Options", "nosniff");
            AddHeader(headers, "Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        }

        private void AddHeader(NameValueCollection headers, string headerName, string headerValue)
        {
            if (!headers.AllKeys.Contains(headerName))
            {
                headers.Add(headerName, headerValue);
            }
        }

        private void RemoveASPNETHeaders(NameValueCollection headers)
        {
            headers.Remove("x-powered-by");
            headers.Remove("x-aspnet-version");
            headers.Remove("x-aspnetmvc-version");
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }
}