using System.Reflection;
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

[assembly: WebActivator.PreApplicationStartMethod(typeof(DDDEastAnglia.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(DDDEastAnglia.App_Start.NinjectWebCommon), "Stop")]

namespace DDDEastAnglia.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(Assembly.GetExecutingAssembly());
            kernel.Bind<IHandle>().To<DeleteVoteCommandHandler>().InRequestScope();
            kernel.Bind<IHandle>().To<RegisterVoteCommandHandler>().InRequestScope();
            kernel.Bind<IMessageBus>().To<SimpleMessageBus>().InRequestScope();
            kernel.Bind<IConferenceRepository>().To<EntityFrameworkConferenceRepository>().InRequestScope();
            kernel.Bind<IVoteRepository>().To<EntityFrameworkVoteRepository>().InRequestScope();
            kernel.Bind<ISessionRepository>().To<EntityFrameworkSessionRepository>().InRequestScope();
            kernel.Bind<ISessionVoteModelQuery>().To<SessionVoteModelQuery>().InRequestScope();
            kernel.Bind<IBannerModelQuery>().To<EntityFrameworkBannerModelQuery>().InRequestScope();
            kernel.Bind<IBuild<Conference, Domain.Conference>>().To<ConferenceBuilder>().InRequestScope();
            kernel.Bind<IBuild<CalendarItem, CalendarEntry>>().To<CalendarEntryBuilder>().InRequestScope();
            kernel.Bind<IBuild<CalendarItem, SingleTimeEntry>>().To<CalendarItemToSingleTimeEntryConverter>().InRequestScope();
            kernel.Bind<IBuild<CalendarItem, TimeRangeEntry>>().To<CalendarItemToTimeRangeEntryConverter>().InRequestScope();
            kernel.Bind<IControllerInformationProvider>().To<HttpContextControllerInformationProvider>().InRequestScope();
        }        
    }
}
