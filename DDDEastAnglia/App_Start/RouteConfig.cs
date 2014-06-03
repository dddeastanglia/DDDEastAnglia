using System.Web.Mvc;
using System.Web.Routing;

namespace DDDEastAnglia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Closed", "Closed", new { controller = "Home", action = "Closed" });
            routes.MapRoute("Preview", "Preview", new { controller = "Home", action = "Preview" });
            routes.MapRoute("Agenda", "Agenda", new { controller = "Home", action = "Agenda" });
            routes.MapRoute("Venue", "Venue", new {controller = "Home", action = "Venue"});

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DDDEastAnglia.Controllers" }
            );
        }
    }
}