using System.Web.Mvc;
using WebMatrix.WebData;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            SimpleRoleProvider provider = new SimpleRoleProvider();
            return null;
        }
    }
}
