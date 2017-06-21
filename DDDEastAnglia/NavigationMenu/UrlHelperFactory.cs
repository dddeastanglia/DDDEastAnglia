using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace DDDEastAnglia.NavigationMenu
{
    public interface IUrlHelperFactory
    {
        UrlHelper Create(RequestContext requestContext);
    }

    public class UrlHelperFactory : IUrlHelperFactory
    {
        public UrlHelper Create(RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException(nameof(requestContext));
            }

            return new UrlHelper(requestContext);
        }
    }
}
