using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();
        
        // GET: /Admin/User/
        public ActionResult Index()
        {
            List<UserProfile> profiles = db.UserProfiles.ToList();
            return null;
        }
    }
}
