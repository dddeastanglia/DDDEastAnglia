using DDDEastAnglia.Helpers.Email.Smtp;
using Ninject.Modules;

namespace DDDEastAnglia.Helpers.Email
{
    public class EmailModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IMailHostSettingsProvider>().To<SmtpHostSettingsProvider>();
        }
    }
}
