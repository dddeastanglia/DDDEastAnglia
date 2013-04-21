using System.Web.Mvc;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public partial class AdminHomeController : Controller
    {
        // GET: /Admin/AdminHome/
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
