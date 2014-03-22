using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class EntityFrameworkDataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(from => from.FromThisAssembly()
                                    .SelectAllClasses()
                                    .InNamespaceOf<ConferenceRepository>()
                                    .BindDefaultInterfaces()
                                    .Configure(binding => binding.InRequestScope()));
        }
    }
}