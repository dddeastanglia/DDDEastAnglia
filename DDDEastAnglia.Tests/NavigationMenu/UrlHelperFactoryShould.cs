using System;
using System.Web.Mvc;
using System.Web.Routing;
using DDDEastAnglia.NavigationMenu;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.NavigationMenu
{
    [TestFixture]
    public sealed class UrlHelperFactoryShould
    {
        [Test]
        public void ThrowAnException_WhenCalledWithANullRequestContext()
        {
            var factory = new UrlHelperFactory();
            Assert.Throws<ArgumentNullException>(() => factory.Create(null));
        }

        [Test]
        public void ReturnAUrlHelper_WhenCalledWithANonNullRequestContext()
        {
            var factory = new UrlHelperFactory();
            var urlHelper = factory.Create(new RequestContext());
            Assert.IsInstanceOf<UrlHelper>(urlHelper);
        }
    }
}
