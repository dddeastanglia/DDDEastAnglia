using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.Models;

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

            return View(roles);
        }

        // GET: /Admin/Role/Manage
        public ActionResult Manage(string id)
        {
            if (Roles.RoleExists(id))
            {
                RoleModel model = new RoleModel { RoleName = id, roleUsers = new SortedList<string, RoleUserModel>() };

                foreach (UserProfile user in db.UserProfiles)
                {
                    if (Roles.IsUserInRole(user.UserName, id))
                    {
                        model.roleUsers.Add(user.UserName,
                            new RoleUserModel() { IsMember = true, UserId = user.UserId, Username = user.UserName });
                    }
                    else
                    {
                        model.roleUsers.Add(user.UserName,
                            new RoleUserModel() { IsMember = false, UserId = user.UserId, Username = user.UserName });

                    }
                }

                return View(model);
            }

            return RedirectToAction("Index", "Role");
        }

        [HttpPost]
        public ActionResult Manage(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (RoleUserModel roleUser in model.roleUsers.Values)
                {
                    if (roleUser.IsMember)
                    {
                        if (!Roles.IsUserInRole(roleUser.Username, model.RoleName))
                        {
                            Roles.AddUserToRole(roleUser.Username, model.RoleName);
                        }
                    }
                    else
                    {
                        if (Roles.IsUserInRole(roleUser.Username, model.RoleName))
                        {
                            Roles.RemoveUserFromRole(roleUser.Username, model.RoleName);
                        }
                    }
                }
            }

            return View("Manage", model);
        }

        // GET: /Admin/Role/Delete
        public ActionResult Delete(string id)
        {
            int roleMembersCount = Roles.GetUsersInRole(id).GetLength(0);
            if (roleMembersCount > 0)
            {
                ViewBag.MembersCount = string.Format("There are currently {0} users in this role.", roleMembersCount);
            }
            else
            {
                Roles.DeleteRole(id);
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: /Admin/Role/Delete

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Roles.DeleteRole(id, false);

            return RedirectToAction("Index");
        }

        // GET: /Admin/Role/Create
        public ActionResult Create()
        {
            return View(string.Empty);
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
                    return View(viewName: "Create", model: role);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
