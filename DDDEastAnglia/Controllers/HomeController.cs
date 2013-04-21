using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public virtual ActionResult Venue()
        {
            return this.View();
        }

        public virtual ActionResult Team()
        {
            return this.View();
        }

        public virtual ActionResult Sponsors()
        {
            return this.View();
        }

        public virtual ActionResult About()
        {
            return this.View();
        }
    }
}
