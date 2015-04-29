using System.Linq;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Sponsors.sponsor_list
{
    [TestFixture]
    public sealed class sponsors_with_same_amount_paid : context
    {
        public sponsors_with_same_amount_paid()
        {
            Given_premium_sponsor(name: "paid nearer the event", paymentDate: 1.October(2015));
            Given_premium_sponsor(name: "paid first", paymentDate: 1.January(2015));
            When_getting_sponsor_list();
        }

        [Test]
        public void Sponsor_who_paid_first_will_appear_higher()
        {
            var names = sponsor_list.Select(sm => sm.Name).ToList();
            Assert.That(names[0], Is.EqualTo("paid first"));
            Assert.That(names[1], Is.EqualTo("paid nearer the event"));
            
        }
    }
}