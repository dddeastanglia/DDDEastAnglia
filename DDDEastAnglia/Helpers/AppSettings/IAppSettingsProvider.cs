namespace DDDEastAnglia.Helpers.AppSettings
{
    public interface IAppSettingsProvider
    {
        string GetSetting(string name);
    }
}