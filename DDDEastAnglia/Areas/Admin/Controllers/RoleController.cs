using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using WebMatrix.WebData;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();
        
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            SimpleRoleProvider provider = new SimpleRoleProvider();
            return null;
        }
    }
}
