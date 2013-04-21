using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using WebMatrix.WebData;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public partial class RoleController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();
        
        // GET: /Admin/Role/
        public virtual ActionResult Index()
        {
            SimpleRoleProvider provider = new SimpleRoleProvider();
            return null;
        }
    }
}
