namespace DDDEastAnglia.Helpers.LoginMethods
{
    public class LoginMethodIconProvider
    {
        public string GetIcon(string providerName)
        {
            switch (providerName)
            {
                case "dddea":
                    return "icon-user";
                case "github":
                    return "icon-github";
                case "twitter":
                    return "icon-twitter";
                case "google":
                    return "icon-google-plus";
                default:
                    return "icon-question-sign";
            }
        }
    }
}
