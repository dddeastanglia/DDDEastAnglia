using System;
using System.Web.Routing;
using DDDEastAnglia.NavigationMenu;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.NavigationMenu
{
    [TestFixture]
    public sealed class MenuStateShould
    {
        [Test]
        public void ThrowAnException_WhenConstructed_IfTheSuppliedRouteDataIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MenuState(null));
        }

        [TestCase("other controller", "this action")]
        [TestCase("this controller", "other action")]
        public void NotBeTheCurrentlySelectedItem_WhenTheControllerNameDoesNotMatch(string controller, string action)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", controller);
            routeData.Values.Add("action", action);
            var menuState = new MenuState(routeData);

            var isCurrentlySelectedItem = menuState.IsCurrentlySelectedItem("this controller", "this action");

            Assert.That(isCurrentlySelectedItem, Is.False);
        }

        [Test]
        public void BeTheCurrentlySelectedItem_WhenTheControllerAndActionNamesMatch()
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "this controller");
            routeData.Values.Add("action", "this action");
            var menuState = new MenuState(routeData);

            var isCurrentlySelectedItem = menuState.IsCurrentlySelectedItem("this controller", "this action");

            Assert.That(isCurrentlySelectedItem, Is.True);
        }
    }
}
