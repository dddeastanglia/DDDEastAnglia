using System.Web.Configuration;

namespace DDDEastAnglia.Helpers.AppSettings
{
    public class WebConfigurationAppSettingsProvider : IAppSettingsProvider
    {
        public string GetSetting(string name)
        {
            return WebConfigurationManager.AppSettings[name];
        }
    }
}