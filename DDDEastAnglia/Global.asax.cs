using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.Builders;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.DataAccess.EntityFramework.Builders;
using DDDEastAnglia.DataAccess.EntityFramework.Builders.Calendar;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataAccess.EntityFramework.Queries;
using DDDEastAnglia.DataAccess.Handlers.Voting;
using DDDEastAnglia.DataAccess.MessageBus;
using DDDEastAnglia.Domain.Calendar;
using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.Context;
using DDDEastAnglia.Models.Query;
using Ninject;
using Ninject.Web.Common;
using WebMatrix.WebData;

namespace DDDEastAnglia
{

    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
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

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IHandle>().To<DeleteVoteCommandHandler>().InRequestScope();
            kernel.Bind<IHandle>().To<RegisterVoteCommandHandler>().InRequestScope();
            kernel.Bind<IMessageBus>().To<SimpleMessageBus>().InRequestScope();
            kernel.Bind<IConferenceRepository>().To<EntityFrameworkConferenceRepository>().InRequestScope();
            kernel.Bind<IVoteRepository>().To<EntityFrameworkVoteRepository>().InRequestScope();
            kernel.Bind<ISessionRepository>().To<EntityFrameworkSessionRepository>().InRequestScope();
            kernel.Bind<ISessionVoteModelQuery>().To<SessionVoteModelQuery>().InRequestScope();
            kernel.Bind<IBannerModelQuery>().To<EntityFrameworkBannerModelQuery>().InRequestScope();
            kernel.Bind<IBuild<Conference, Domain.Conference>>().To<ConferenceBuilder>();
            kernel.Bind<IBuild<CalendarItem, CalendarEntry>>().To<CalendarEntryBuilder>();
            kernel.Bind<IBuild<CalendarItem, SingleTimeEntry>>().To<CalendarItemToSingleTimeEntryConverter>();
            kernel.Bind<IBuild<CalendarItem, TimeRangeEntry>>().To<CalendarItemToTimeRangeEntryConverter>();
            kernel.Bind<IControllerInformationProvider>().To<HttpContextControllerInformationProvider>();
            return kernel;
        }
    }
}