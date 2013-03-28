using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    public partial class RoleController : Controller
    {
        //
        // GET: /Admin/Role/

        public virtual ActionResult Index()
        {
            SimpleRoleProvider provider = new SimpleRoleProvider();
            return null;
            // return View();
        }

    }
}
