using System;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.DataAccess
{
    [TestFixture]
    public sealed class SponsorSorterTests
    {
        [Test]
        public void SponsorsAreOrderedByAmount_WithHighestAmountFirst()
        {
            var poorSponsor = new Sponsor { SponsorshipAmount = 100 };
            var averageSponsor = new Sponsor { SponsorshipAmount = 1000 };
            var richSponsor = new Sponsor { SponsorshipAmount = 100000 };
            var sorter = new SponsorSorter();

            var sortedSponsors = sorter.Sort(new[] { averageSponsor, richSponsor, poorSponsor });

            Assert.That(sortedSponsors, Is.EqualTo(new[] { richSponsor, averageSponsor, poorSponsor }));
        }

        [Test]
        public void SponsorsAreOrderedPaymentDate_WhenSponsoringTheSameAmount()
        {
            var earlySponsor = new Sponsor { SponsorshipAmount = 100, PaymentDate = new DateTime(2015, 4, 1) };
            var sponsor = new Sponsor { SponsorshipAmount = 100, PaymentDate = new DateTime(2015, 4, 15) };
            var lastMinuteSponsor = new Sponsor { SponsorshipAmount = 100, PaymentDate = new DateTime(2015, 4, 30) };
            var sorter = new SponsorSorter();

            var sortedSponsors = sorter.Sort(new[] { sponsor, lastMinuteSponsor, earlySponsor });

            Assert.That(sortedSponsors, Is.EqualTo(new[] { earlySponsor, sponsor, lastMinuteSponsor }));
        }
    }
}
