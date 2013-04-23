using System.Web.Mvc;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminHomeController : Controller
    {
        // GET: /Admin/AdminHome/
        public ActionResult Index()
        {
            return View();
        }
    }
}
