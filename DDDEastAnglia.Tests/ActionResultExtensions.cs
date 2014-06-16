using System.Net;
using System.Web.Mvc;

namespace DDDEastAnglia.Tests
{
    public static class ActionResultExtensions
    {
        public static T GetViewModel<T>(this ActionResult actionResult)
        {
            var result = (ViewResult) actionResult;
            var model = (T) result.Model;
            return model;
        }

        public static string GetRedirectionViewName(this ActionResult actionResult)
        {
            var result = (RedirectToRouteResult) actionResult;
            var viewName = result.RouteValues["action"];
            return viewName.ToString();
        }

        public static HttpStatusCode GetHttpStatusCode(this ActionResult actionResult)
        {
            var httpStatusCodeResult = (HttpStatusCodeResult) actionResult;
            return (HttpStatusCode) httpStatusCodeResult.StatusCode;
        }
    }
}
