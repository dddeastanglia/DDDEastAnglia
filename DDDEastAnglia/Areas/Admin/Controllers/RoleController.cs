using System.Collections.Generic;
using System.Web.Mvc;
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
            RoleManager manager = new RoleManager();

            List<string> roles = new List<string>();
            roles.AddRange(manager.GetAllRoles());

            return View(roles);
        }

        // GET: /Admin/Role/Manage
        public ActionResult Manage(string rolename)
        {
            RoleManager manager = new RoleManager();

            if (manager.RoleExists(rolename))
            {
                ManageRoleModel model = new ManageRoleModel { RoleName = rolename, roleUsers = new SortedList<string, RoleUserModel>() };

                foreach (UserProfile user in _db.UserProfiles)
                {
                    if (manager.IsUserInRole(user.UserName, rolename))
                    {
                        model.roleUsers.Add(user.UserName,
                            new RoleUserModel { IsMember = true, UserId = user.UserId, Username = user.UserName });
                    }
                    else
                    {
                        model.roleUsers.Add(user.UserName,
                            new RoleUserModel { IsMember = false, UserId = user.UserId, Username = user.UserName });
                    }
                }

                return View(model);
            }

            return RedirectToAction("Index", "Role");
        }

        [HttpPost]
        public ActionResult Manage(ManageRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            foreach (RoleUserModel roleUser in model.roleUsers.Values)
            {
                AddRemoveRoleMember(model.RoleName, roleUser);
            }

            return View("Manage", model);
        }

        private static void AddRemoveRoleMember(string rolename, RoleUserModel roleUser)
        {
            RoleManager manager = new RoleManager();

            if (roleUser.IsMember)
            {
                if (!manager.IsUserInRole(roleUser.Username, rolename))
                {
                    manager.AddUserToRole(roleUser.Username, rolename);
                }
            }
            else
            {
                if (manager.IsUserInRole(roleUser.Username, rolename))
                {
                    manager.RemoveUserFromRole(roleUser.Username, rolename);
                }
            }
        }

        // GET: /Admin/Role/Delete
        public ActionResult Delete(string rolename)
        {
            RoleManager manager = new RoleManager();

            DeleteRoleModel model = new DeleteRoleModel();

            int roleMembersCount = manager.GetUsersCount(rolename);
            if (roleMembersCount > 0)
            {
                model.FeedbackMessage = string.Format("There are currently {0} users in this role.", roleMembersCount);
                return View(model);
            }

            manager.DeleteRole(rolename);

            return RedirectToAction("Index");

        }

        // POST: /Admin/Role/Delete

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string rolename)
        {
            RoleManager manager = new RoleManager();

            manager.DeleteRole(rolename);

            return RedirectToAction("Index");
        }

        // GET: /Admin/Role/Create
        public ActionResult Create()
        {
            CreateRoleModel model = new CreateRoleModel { RoleName = string.Empty };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateRoleModel model)
        {
            RoleManager manager = new RoleManager();

            if (ModelState.IsValid)
            {
                if (!manager.RoleExists(model.RoleName))
                {
                    manager.CreateRole(model.RoleName);
                }
                else
                {
                    model.FeedbackMessage = "This role already exists!";
                    return View("Create", model);
                }
            }

            return RedirectToAction("Index");
        }

    }
}
