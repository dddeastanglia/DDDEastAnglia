using System;
using System.Web.Routing;
using DDDEastAnglia.NavigationMenu;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.NavigationMenu
{
    [TestFixture]
    public sealed class MenuStateFactoryShould
    {
        [Test]
        public void ThrowAnException_WhenCalledWithANullRouteData()
        {
            var factory = new MenuStateFactory();
            Assert.Throws<ArgumentNullException>(() => factory.Create(null));
        }

        [Test]
        public void ReturnAMenuState_WhenCalledWithANonNullRouteData()
        {
            var factory = new MenuStateFactory();
            var menuState = factory.Create(new RouteData());
            Assert.IsNotNull(menuState);
        }
    }
}
