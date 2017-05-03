using System.Web.Mvc;
using DDDEastAnglia.Filters;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Filters
{
    [TestFixture]
    public sealed class ConferenceIsClosedFilterTests
    {
        [Test]
        public void Filter_RedirectsToClosedPage_WhenTheConferenceIsClosed()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WhenClosed()
                                        .Build();
            var filterAttribute = new ConferenceIsClosedFilter(conferenceLoader);
            var filterContext = new ActionExecutingContext();
            
            filterAttribute.OnActionExecuting(filterContext);

            Assert.That(filterContext.Result.GetRedirectionUrl(), Contains.Substring("Closed"));
        }

        [Test]
        public void Filter_DoesNotRedirectToClosedPage_WhenTheConferenceIsNotClosed()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .WhenNotClosed()
                                        .Build();
            var filterAttribute = new ConferenceIsClosedFilter(conferenceLoader);
            var filterContext = new ActionExecutingContext();
            
            filterAttribute.OnActionExecuting(filterContext);

            Assert.That(filterContext.Result, Is.Not.InstanceOf<RedirectResult>());
        }
    }
}
