using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;

namespace DDDEastAnglia.Tests
{
    public static class ControllerTestExtensions
    {
        public static void CreateModelStateError(this Controller controller)
        {
            controller.ModelState.AddModelError("an error", "there was an error");
        }

        /// <summary>
        /// Sets up a controller so that it has an HttpContext and a Url property.
        /// </summary>
        public static void SetupWithHttpContextAndUrlHelper(this Controller controller)
        {
            var httpContext = Substitute.For<HttpContextBase>();
            var httpRequest = Substitute.For<HttpRequestBase>();
            httpRequest.Url.Returns(new Uri("https://example.com"));
            httpContext.Request.Returns(httpRequest);
            var controllerContext = new ControllerContext {HttpContext = httpContext};
            controller.ControllerContext = controllerContext;
            controller.Url = new UrlHelper(new RequestContext(controller.HttpContext, new RouteData()), RouteTable.Routes);
        }
    }
}
