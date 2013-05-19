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
                ManageRoleModel model = new ManageRoleModel { RoleName = id, roleUsers = new SortedList<string, RoleUserModel>() };

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
        public ActionResult Manage(ManageRoleModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View("Manage", model);
            }

            foreach (RoleUserModel roleUser in model.roleUsers.Values)
            {
                AddRemoveRoleMember(model.RoleName, roleUser);
            }

            return View("Manage", model);
        }

        private static void AddRemoveRoleMember(string Role, RoleUserModel roleUser)
        {
            if (roleUser.IsMember)
            {
                if (!Roles.IsUserInRole(roleUser.Username, Role))
                {
                    Roles.AddUserToRole(roleUser.Username, Role);
                }
            }
            else
            {
                if (Roles.IsUserInRole(roleUser.Username, Role))
                {
                    Roles.RemoveUserFromRole(roleUser.Username, Role);
                }
            }
        }

        // GET: /Admin/Role/Delete
        public ActionResult Delete(string id)
        {
            DeleteRoleModel model = new DeleteRoleModel();

            int roleMembersCount = Roles.GetUsersInRole(id).GetLength(0);
            if (roleMembersCount > 0)
            {
                model.FeedbackMessage = string.Format("There are currently {0} users in this role.", roleMembersCount);
                return View(model);
            }

            Roles.DeleteRole(id);
            return RedirectToAction("Index");

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
            CreateRoleModel model = new CreateRoleModel() { RoleName = string.Empty };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Roles.RoleExists(model.RoleName))
                {
                    Roles.CreateRole(model.RoleName);
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
