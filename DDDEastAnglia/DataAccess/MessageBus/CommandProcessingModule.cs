using DDDEastAnglia.DataAccess.Handlers.Voting;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DDDEastAnglia.DataAccess.MessageBus
{
    public class CommandProcessingModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IHandle>().To<DeleteVoteCommandHandler>().InRequestScope();
            Kernel.Bind<IHandle>().To<RegisterVoteCommandHandler>().InRequestScope();
            Kernel.Bind<IMessageBus>().To<SimpleMessageBus>().InRequestScope();
        }
    }
}