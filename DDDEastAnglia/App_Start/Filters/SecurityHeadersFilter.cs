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
            HttpRequestBase requestBase = contextBase.Request;
            HttpResponseBase responseBase = contextBase.Response;
            NameValueCollection headers = responseBase.Headers;

            AddSecurityHeaders(headers, requestBase.IsSecureConnection);
        }

        private void AddSecurityHeaders(NameValueCollection headers, bool isSecure)
        {
            AddHeader(headers, "X-Frame-Options", "SAMEORIGIN");
            AddHeader(headers, "X-XSS-Protection", "1; mode=block");
            AddHeader(headers, "X-Content-Type-Options", "nosniff");
            if (isSecure)
            {
                AddHeader(headers, "Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            }
        }

        private void AddHeader(NameValueCollection headers, string headerName, string headerValue)
        {
            if (!headers.AllKeys.Contains(headerName))
            {
                headers.Add(headerName, headerValue);
            }
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }
    }
}