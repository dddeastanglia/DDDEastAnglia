using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private readonly IRoleManager _manager;
        private readonly IUserProfileRepository _userProfileRepository;

        public RoleController(IRoleManager roleManager, IUserProfileRepository userProfileRepository)
        {
            if (roleManager == null)
            {
                throw new ArgumentNullException("roleManager");
            }
            _manager = roleManager;

            if (userProfileRepository == null)
            {
                throw new ArgumentNullException("userProfileRepository");
            }
            _userProfileRepository = userProfileRepository;
        }

        // GET: /Admin/Role/
        public ActionResult Index()
        {
            List<string> roles = new List<string>();
            roles.AddRange(_manager.GetAllRoles());

            return View(roles);
        }

        public ActionResult Rename(string rolename)
        {
            return View(new RoleModel { RoleName = rolename });
        }

        [HttpPost]
        public ActionResult Rename([Bind(Exclude = "AvailableUsers, FeedbackMessage")]RoleModel model)
        {
            if (!ModelState.IsValid) return View(model);
            _manager.RenameRole(model.RoleName, model.NewRoleName);
            return RedirectToAction("Index");
        }

        public ActionResult AddUsers(string rolename)
        {
            RoleModel model = new RoleModel { RoleName = rolename };
            // Get the list of users no currently in the group
            List<string> roleUserList = _manager.GetUsersForRole(rolename).ToList();
            List<string> userList = _userProfileRepository.GetAllUserProfiles().Select(n => n.UserName).ToList();
            List<string> availableUsers = userList.Except(roleUserList).ToList();

            if (availableUsers.Count == 0)
            {
                model.FeedbackMessage = "There are no users that can be added to this group";
            }
            else
            {
                foreach (string availableUser in availableUsers)
                {
                    model.AvailableUsers.Add(availableUser, false);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddUsers([Bind(Exclude = "NewRoleName,FeedbackMessage")]RoleModel model)
        {
            if (!ModelState.IsValid) return View(model);
            foreach (KeyValuePair<string, bool> availableUser in model.AvailableUsers)
            {
                if (availableUser.Value && !_manager.IsUserInRole(availableUser.Key, model.RoleName))
                {
                    _manager.AddUserToRole(availableUser.Key, model.RoleName);
                }
            }
            return RedirectToAction("Index");
        }

        // GET: /Admin/Role/Delete
        public ActionResult Delete(string rolename)
        {
            RoleModel model = new RoleModel();

            int roleMembersCount = _manager.GetUsersCount(rolename);
            if (roleMembersCount > 0)
            {
                model.FeedbackMessage = string.Format("There are currently {0} users in this role.", roleMembersCount);
                return View(model);
            }

            _manager.DeleteRole(rolename);

            return RedirectToAction("Index");

        }

        // POST: /Admin/Role/Delete

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string rolename)
        {
            _manager.DeleteRole(rolename);

            return RedirectToAction("Index");
        }

        // GET: /Admin/Role/Create
        public ActionResult Create()
        {
            RoleModel model = new RoleModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_manager.RoleExists(model.RoleName))
                {
                    _manager.CreateRole(model.RoleName);
                }
                else
                {
                    model.FeedbackMessage = "This role already exists!";
                    return View("Create", model);
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(string rolename)
        {
            RoleModel model = new RoleModel { RoleName = rolename };
            string[] users = _manager.GetUsersForRole(rolename);
            foreach (string user in users)
            {
                model.AvailableUsers.Add(user, true);
            }
            return View(model);
        }

        public ActionResult RemoveUsers(string rolename)
        {
            RoleModel model = new RoleModel { RoleName = rolename };
            string[] users = _manager.GetUsersForRole(rolename);

            if (!users.Any())
            {
                model.FeedbackMessage = "There are no users that can be removed from this role.";
            }
            else
            {
                foreach (string user in users)
                {
                    model.AvailableUsers.Add(user, true);
                }
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult RemoveUsers([Bind(Exclude = "NewRoleName,FeedbackMessage")]RoleModel model)
        {
            if (!ModelState.IsValid) return View(model);
            foreach (KeyValuePair<string, bool> availableUser in model.AvailableUsers)
            {
                if (!availableUser.Value && _manager.IsUserInRole(availableUser.Key, model.RoleName)) _manager.RemoveUserFromRole(availableUser.Key, model.RoleName);
            }
            return RedirectToAction("Index");
        }
    }
}
