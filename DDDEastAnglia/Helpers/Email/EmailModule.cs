using System.IO;
using System.Reflection;
using DDDEastAnglia.Helpers.Email.SendGrid;
using DDDEastAnglia.Helpers.Email.Smtp;
using Ninject.Modules;

namespace DDDEastAnglia.Helpers.Email
{
    public class EmailModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IMailHostSettingsProvider>().To<SmtpHostSettingsProvider>();
            Kernel.Bind<IRenderer>()
                .ToMethod(x =>
                {
                    var htmlEmailTemplateFileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DDDEastAnglia.EmailTemplate.html");
                    using (StreamReader reader = new StreamReader(htmlEmailTemplateFileStream))
                    {
                        var htmlEmailTemplate = reader.ReadToEnd();
                        return new HtmlRenderer(htmlEmailTemplate);
                    }
                })
                .InSingletonScope();
        }
    }
}
