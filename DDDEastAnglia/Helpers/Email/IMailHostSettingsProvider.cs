namespace DDDEastAnglia.Helpers.Email
{
    public interface IMailHostSettingsProvider
    {
        IMailHostSettings GetSettings();
    }
}