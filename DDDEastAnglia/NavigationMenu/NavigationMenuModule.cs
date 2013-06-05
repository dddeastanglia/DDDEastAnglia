using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.NavigationMenu
{
    public class NavigationMenuModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(from => from.FromThisAssembly()
                                    .SelectAllClasses()
                                    .InNamespaceOf<MenuStateFactory>()
                                    .BindDefaultInterfaces()
                                    .Configure(binding => binding.InRequestScope()));
        }
    }
}