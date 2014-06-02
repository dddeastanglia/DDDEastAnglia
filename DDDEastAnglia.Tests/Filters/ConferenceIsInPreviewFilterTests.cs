using System.Web.Mvc;
using DDDEastAnglia.Filters;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Filters
{
    [TestFixture]
    public sealed class ConferenceIsInPreviewFilterTests
    {
        [Test]
        public void Filter_RedirectsToPreviewPage_WhenTheConferenceIsInPreview()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .InPreview()
                                        .Build();
            var filterAttribute = new ConferenceIsInPreviewFilterAttribute(conferenceLoader);
            var filterContext = new ActionExecutingContext();
            
            filterAttribute.OnActionExecuting(filterContext);

            Assert.That(filterContext.Result.GetRedirectionUrl(), Contains.Substring("Preview"));
        }

        [Test]
        public void Filter_DoesNotRedirectToPreviewPage_WhenTheConferenceIsNotInPreview()
        {
            var conferenceLoader = new ConferenceLoaderBuilder()
                                        .Build();
            var filterAttribute = new ConferenceIsInPreviewFilterAttribute(conferenceLoader);
            var filterContext = new ActionExecutingContext();
            
            filterAttribute.OnActionExecuting(filterContext);

            Assert.That(filterContext.Result, Is.Not.InstanceOf<RedirectResult>());
        }
    }
}
