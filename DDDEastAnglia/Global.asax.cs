using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

using WebMatrix.WebData;

namespace DDDEastAnglia
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
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
            Database.SetInitializer(new Initialiser());
            using (var context = new DDDEAContext())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Initialize(false);
                }
            }
            WebSecurity.InitializeDatabaseConnection("DDDEastAnglia", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }
    }
}