using System.Web.Mvc;
using DDDEastAnglia.Models.Query;

namespace DDDEastAnglia.Controllers
{
    public class BannerController : Controller
    {
        private readonly IBannerModelQuery bannerQuery;

        public BannerController(IBannerModelQuery bannerQuery)
        {
            this.bannerQuery = bannerQuery;
        }

        public ActionResult Details()
        {
            return PartialView("_Banner", bannerQuery.Get());
        }
    }
}