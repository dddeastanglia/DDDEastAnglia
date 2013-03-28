using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    public partial class AdminHomeController : Controller
    {
        //
        // GET: /Admin/AdminHome/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
