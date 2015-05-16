using DDDEastAnglia.Helpers;
using NSubstitute;

namespace DDDEastAnglia.Tests.Builders
{
    public class DnsLookupBuilder
    {
        private readonly IDnsLookup dnsLookup;

        public DnsLookupBuilder()
        {
            dnsLookup = Substitute.For<IDnsLookup>();
        }

        public DnsLookupBuilder WithIPAddressResolvingTo(string ipAddress, string website)
        {
            dnsLookup.Resolve(ipAddress).Returns(website);
            return this;
        }

        public IDnsLookup Build()
        {
            return dnsLookup;
        }
    }
}
