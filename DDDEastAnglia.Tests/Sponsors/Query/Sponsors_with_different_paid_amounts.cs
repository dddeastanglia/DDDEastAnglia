using System.Linq;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Sponsors.Query
{
    [TestFixture]
    public sealed class Sponsors_with_different_paid_amounts : Context
    {
        public Sponsors_with_different_paid_amounts()
        {
            Given_gold_sponsor(name: "middleOfTheRoad");
            Given_standard_sponsor(name: "Cheapest");
            Given_premium_sponsor(name: "big Spender");
            When_getting_sponsor_list();
        }

        [Test]
        public void Sponsors_are_ordered_by_amount_desc()
        {
            var names = SponsorList.Select(sm => sm.Name);
            CollectionAssert.AreEqual(new[] { "big Spender", "middleOfTheRoad", "Cheapest" }, names);
        }
    }
}