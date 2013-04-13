namespace DDDEastAnglia.Helpers
{
    public interface IRequestInformationProvider
    {
        string GetIPAddress();
    }

    public class HttpContextRequestInformationProvider : IRequestInformationProvider
    {
        public string GetIPAddress()
        {
            throw new System.NotImplementedException();
        }
    }
}