using System.Net;

namespace DDDEastAnglia.Helpers
{
    public class DnsLookup
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