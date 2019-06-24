using System.Configuration;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DDDEastAnglia.DatabaseMigrations;
using WebMatrix.WebData;

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

            InitialiseDatabase();

            MvcHandler.DisableMvcResponseHeader = true;
        }

        private static void InitialiseDatabase()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["DDDEastAnglia"];
            var databaseMigrator = new Migrator(connectionStringSettings.ConnectionString);
            databaseMigrator.MigrateToLatestSchema();

            WebSecurity.InitializeDatabaseConnection("DDDEastAnglia", "UserProfiles", "UserId", "UserName", autoCreateTables: false);
        }
    }
}
