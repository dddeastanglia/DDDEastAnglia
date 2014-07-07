using System.Web.Mvc;
using DDDEastAnglia.Helpers;

namespace DDDEastAnglia.Controllers
{
    public class GravatarController : Controller
    {
        public ContentResult Url(string emailAddress, bool useIdenticon = false, int size = 50)
        {
            var url = new GravatarUrl().GetUrl(emailAddress, useIdenticon: useIdenticon, size: size);
            return new ContentResult { Content = url };
        }
    }
}
