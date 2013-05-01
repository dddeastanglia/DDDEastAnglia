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
                                    .InNamespaceOf<EntityFrameworkConferenceRepository>()
                                    .BindDefaultInterfaces()
                                    .Configure(binding => binding.InRequestScope()));
        }
    }
}