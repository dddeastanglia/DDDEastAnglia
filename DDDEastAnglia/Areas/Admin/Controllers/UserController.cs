using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DDDEastAnglia.Models;

using WebMatrix.WebData;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    public partial class UserController : Controller
    {
        private UsersContext db = new UsersContext();
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
