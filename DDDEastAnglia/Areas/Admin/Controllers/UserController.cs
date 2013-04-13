using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.DataAccess.EntityFramework;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    public partial class UserController : Controller
    {
        private DDDEAContext db = new DDDEAContext();
        //
        // GET: /Admin/User/

        public virtual ActionResult Index()
        {
            List<UserProfile> profiles = db.UserProfiles.ToList();
            return null;
            // return View();
        }

    }
}
