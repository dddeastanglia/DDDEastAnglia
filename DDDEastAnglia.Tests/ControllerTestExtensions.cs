using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DDDEastAnglia.Models;
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

        public static void SetupWithAuthenticatedUser(this Controller controller, UserProfile user)
        {
            var userIdentity = Substitute.For<IIdentity>();
            userIdentity.Name.Returns(user.UserName);

            var userPrincipal = Substitute.For<IPrincipal>();
            userPrincipal.Identity.Returns(userIdentity);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.User.Returns(userPrincipal);

            controller.ControllerContext = new ControllerContext {  HttpContext = httpContext };
        }
    }
}
