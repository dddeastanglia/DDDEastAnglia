using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public partial class SpeakerController : Controller
    {
        //
        // GET: /Speaker/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
