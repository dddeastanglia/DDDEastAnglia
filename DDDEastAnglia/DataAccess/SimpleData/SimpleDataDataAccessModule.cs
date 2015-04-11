using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.DataAccess.SimpleData
{
    public class SimpleDataDataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(from => from.FromThisAssembly()
                                    .SelectAllClasses()
                                    .InNamespaceOf<ConferenceRepository>()
                                    .BindDefaultInterfaces()
                                    .Configure(binding => binding.InRequestScope()));
            Kernel.Bind<IConferenceLoader>().To<ConferenceLoader>().InRequestScope();
            Kernel.Bind<ISpeakerRepository>().To<SpeakerRepository>().InRequestScope();
            Kernel.Bind<ISponsorSorter>().To<SponsorSorter>().InRequestScope();
            Kernel.Bind<IVotingCookieFactory>().To<CachingVotingCookieFactory>().InRequestScope();
            Kernel.Bind<IVotingCookieFactory>().To<VotingCookieFactory>().WhenInjectedInto<CachingVotingCookieFactory>().InRequestScope();
        }
    }
}
