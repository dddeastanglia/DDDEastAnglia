using System;
using System.Web;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Sessions;
using DDDEastAnglia.VotingData;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DDDEastAnglia.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DDDEastAnglia.App_Start.NinjectWebCommon), "Stop")]

namespace DDDEastAnglia.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<IDateTimeOffsetProvider>().To<LocalDateTimeOffsetProvider>();
            kernel.Bind<IDataProvider>().To<DataProvider>();
            kernel.Bind<ISessionLoader>().ToMethod(CreateSessionLoader);

            RegisterServices(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load(typeof(NinjectWebCommon).Assembly);
        }

        private static ISessionLoader CreateSessionLoader(IContext context)
        {
            var conferenceLoader = context.Kernel.Get<IConferenceLoader>();
            var conference = conferenceLoader.LoadConference();

            var sessionRepository = context.Kernel.Get<ISessionRepository>();

            var factory = new SessionLoaderFactory(sessionRepository);
            return factory.Create(conference);        }
    }
}
