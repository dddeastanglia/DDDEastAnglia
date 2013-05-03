using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        // GET: /Admin/Role/
        public ActionResult Index()
        {
            List<string> roles = new List<string>();
            roles.AddRange(Roles.GetAllRoles());

            return this.View(roles);
        }

        // GET: /Admin/Role/Manage
        public ActionResult Manage(string id)
        {
            return this.View();
        }

        // GET: /Admin/Role/Delete
        public ActionResult Delete(string id)
        {
            return this.View();
        }

        // GET: /Admin/Role/Create
        public ActionResult Create()
        {
            string role = string.Empty;

            return this.View(role);
        }

        [HttpPost]
        public ActionResult Create(string role)
        {
            if (ModelState.IsValid)
            {
                if (!Roles.RoleExists(role))
                {
                    Roles.CreateRole(role);
                }
                else
                {
                    ViewBag["Message"] = "This role already exists!";
                    return this.View(role);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
