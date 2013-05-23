namespace DDDEastAnglia.Helpers.Email
{
    public interface IMailHostSettings
    {
        string Host{get;}
        int Port{get;}
        string Username{get;}
        string Password{get;}
    }
}
