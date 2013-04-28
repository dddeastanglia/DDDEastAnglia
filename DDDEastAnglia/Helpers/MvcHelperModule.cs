using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Helpers.Context;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.Helpers
{
    public class MvcHelperModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISessionVoteModelQuery>().To<SessionVoteModelQuery>().InRequestScope();
            Kernel.Bind<IControllerInformationProvider>().To<HttpContextControllerInformationProvider>().InRequestScope();
        }
    }
}