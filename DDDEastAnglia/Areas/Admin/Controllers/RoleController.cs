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
            int roleMembersCount = Roles.GetUsersInRole(id).GetLength(0);
            if (roleMembersCount > 0)
            {
                ViewBag.MembersCount = string.Format("There are currently {0} users in this role.", roleMembersCount);
            }

            return this.View();
        }

        // POST: /Admin/Role/Delete

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Roles.DeleteRole(id);

            return RedirectToAction("Index");
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
                    ViewBag.Message = "This role already exists!";
                    return this.View(viewName: "Create", model: role);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
