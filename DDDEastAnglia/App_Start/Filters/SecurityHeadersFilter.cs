using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;

namespace DDDEastAnglia.App_Start.Filters
{
    public class SecurityHeadersFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            HttpContextBase contextBase = filterContext.HttpContext;
            HttpResponseBase responseBase = contextBase.Response;
            NameValueCollection headers = responseBase.Headers;

            RemoveASPNETHeaders(headers);
        }

        private void RemoveASPNETHeaders(NameValueCollection headers)
        {
            headers.Remove("x-powered-by");
            headers.Remove("x-aspnet-version");
            headers.Remove("x-aspnetmvc-version");
        }
    }
}