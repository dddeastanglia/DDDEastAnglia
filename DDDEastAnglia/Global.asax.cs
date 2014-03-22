using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DDDEastAnglia
{

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            CreateDatabaseIfNecessary();
        }

        private static void CreateDatabaseIfNecessary()
        {
//            Database.SetInitializer(new Initialiser());
//            using (var context = new DDDEAContext())
//            {
//                if (!context.Database.Exists())
//                {
//                    context.Database.Initialize(false);
//                }
//            }
//            WebSecurity.InitializeDatabaseConnection("DDDEastAnglia", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }
    }
}
