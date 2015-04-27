using System.Linq;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Sponsors.sponsor_list
{
    [TestFixture]
    public sealed class Sponsors_with_different_paid_amounts : context
    {
        public Sponsors_with_different_paid_amounts()
        {
            Given_sponsor(name: "middleOfTheRoad", amount: 1000);
            Given_sponsor(name: "Cheapest", amount: 200);
            Given_sponsor(name: "big Spender", amount: 1500);
            When_getting_sponsor_list();
        }

        [Test]
        public void Sponsors_are_ordered_by_amount_desc()
        {
            var names = sponsor_list.Select(sm => sm.Name);
            CollectionAssert.AreEqual(new[] { "big Spender", "middleOfTheRoad", "Cheapest" }, names);
        }
    }
}