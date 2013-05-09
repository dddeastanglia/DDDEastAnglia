using System.Net;

namespace DDDEastAnglia.Helpers
{
    public interface IDnsLookup
    {
        string Resolve(string ipAddress);
    }

    public class DnsLookup : IDnsLookup
    {
        public const string UnknownHostName = "[unknown]";

        public string Resolve(string ipAddress)
        {
            string hostName;

            try
            {
                var ipHostEntry = Dns.GetHostEntry(ipAddress);
                hostName = ipHostEntry.HostName;
            }
            catch
            {
                hostName = UnknownHostName;
            }

            return hostName;
        }
    }
}