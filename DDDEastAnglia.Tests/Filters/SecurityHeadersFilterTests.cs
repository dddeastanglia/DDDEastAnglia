using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DDDEastAnglia.App_Start.Filters;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Controllers;
using DDDEastAnglia.DataAccess;
using NSubstitute;
using NUnit.Framework;

namespace DDDEastAnglia.Tests.Filters
{

    [TestFixture]
    public class SecurityHeadersFilterTests
    {
        [Test]
        public void XPoweredBy_Header_Is_Removed()
        {
            HttpContextBase contextBase = Substitute.For<HttpContextBase>();
            HttpResponseBase responseBase = Substitute.For<HttpResponseBase>();
            NameValueCollection headers = new NameValueCollection {{"X-Powered-By", "ASP.NET"}};
            responseBase.Headers.Returns(headers);
            contextBase.Response.Returns(responseBase);

            ResultExecutingContext context = new ResultExecutingContext
            {
                HttpContext = contextBase
            };

            SecurityHeadersFilter filter = new SecurityHeadersFilter();

            filter.OnResultExecuting(context);

            NameValueCollection filteredHeaders = responseBase.Headers;
            
            Assert.That(filteredHeaders["X-Powered-By"], Is.Null);
        }
    }
}