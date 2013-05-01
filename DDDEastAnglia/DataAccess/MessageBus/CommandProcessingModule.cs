using Ninject.Extensions.Conventions;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.DataAccess.MessageBus
{
    public class CommandProcessingModule : NinjectModule
    {
        public override void Load()
        {
            // Once we have something better for handling the unit of work then these could move to being in the
            // singleton scope as the handler defines the transaction boundary but until then the request 
            // should be defining the transaction boundary.
            Kernel.Bind(from => from.FromThisAssembly()
                                    .SelectAllClasses()
                                    .InNamespaceOf<CommandProcessingModule>()
                                    .BindDefaultInterfaces()
                                    .Configure(binding => binding.InRequestScope()));
            
            Kernel.Bind(from => from.FromThisAssembly()
                                    .SelectAllClasses()
                                    .InheritedFrom<IHandle>()
                                    .BindDefaultInterfaces()
                                    .Configure(binding => binding.InRequestScope()));
        }
    }
}