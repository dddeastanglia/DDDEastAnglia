using DDDEastAnglia.Helpers;
using DDDEastAnglia.Helpers.HttpContext;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Helpers
{
    [TestFixture]
    public class Given_An_IPV4_Address_MatchIPAddress_Should
    {
        [TestCase("123.123.123.123", "123.123.123.123")]
        [TestCase("88.123.123.123", "88.123.123.123")]
        [TestCase("88.123.123.123", "88.123.123.123")]
        [TestCase("123.88.123.123", "123.88.123.123")]
        [TestCase("127.0.0.1", "127.0.0.1")]
        [TestCase("This is IPAddress{127.0.0.1} more stuff", "127.0.0.1")]
        [TestCase("127.0.0.1, 127.0.0.2, 127.0.0.3", "127.0.0.1")]
        public void Return_The_Valid_Address(string inputString, string expectedOutput)
        {
            var result = HttpContextRequestInformationProvider.MatchIPAddress(inputString);

            Assert.That(result, Is.EqualTo(expectedOutput));
        }

        [TestCase("127...1")]
        [TestCase("127.1..")]
        [TestCase("256.0.0.1")]
        [TestCase("1.256.1.1")]
        [TestCase("1.1.256.1")]
        [TestCase("1.1.1.256")]
        [TestCase("400.400.400.400")]
        public void Return_Null_If_The_Address_Is_Invalid(string input)
        {
            Assert.That(HttpContextRequestInformationProvider.MatchIPAddress(input), Is.Null);
        }
    }
}