using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace DDDEastAnglia.Controllers
{
    public class ConnectionStringsController : Controller
    {
        //
        // GET: /ConnectionStrings/

        public ActionResult Index()
        {
            Dictionary<string, string> connectionStrings = new Dictionary<string, string>();

            foreach (dynamic connectionString in WebConfigurationManager.ConnectionStrings)
            {
                connectionStrings.Add(connectionString.Name, connectionString.ConnectionString);
            }

            return View(connectionStrings);
        }

    }
}
