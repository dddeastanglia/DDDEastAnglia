using System.Web.Mvc;

namespace DDDEastAnglia.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "AdminHome", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "DDDEastAnglia.Areas.Admin.Controllers" }
            );
        }
    }
}
