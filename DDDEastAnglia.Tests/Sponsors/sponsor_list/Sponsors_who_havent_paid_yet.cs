using System.Linq;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Sponsors.sponsor_list
{
    [TestFixture]
    public sealed class Sponsors_who_havent_paid_yet : context
    {
        public Sponsors_who_havent_paid_yet()
        {
            Given_unpaid_sponsor(name: "waiting for budget");
            Given_standard_sponsor(name: "all paid up");
            Given_premium_sponsor(name: "big Spender");
            When_getting_sponsor_list();
        }

        [Test]
        public void Unpaid_sponsor_is_hidden()
        {
            var names = sponsor_list.Select(sm => sm.Name);
            CollectionAssert.DoesNotContain(names, "waiting for budget");
        }

        [Test]
        public void Paid_sponsors_are_displayed()
        {
            var names = sponsor_list.Select(sm => sm.Name);
            CollectionAssert.AreEqual(new[] { "big Spender", "all paid up" }, names);
        }
    }
}