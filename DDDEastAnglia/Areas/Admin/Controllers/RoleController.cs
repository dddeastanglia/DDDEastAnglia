using DDDEastAnglia.Areas.Admin.Models;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;
using System;
using System.Collections.Generic;
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

        // GET: /Admin/Role/Manage
        public ActionResult Manage(string rolename)
        {
            if (_manager.RoleExists(rolename))
            {
                RoleModel model = new RoleModel { RoleName = rolename, RoleUsers = new SortedList<string, RoleUserModel>() };

                foreach (UserProfile user in _userProfileRepository.GetAllUserProfiles())
                {
                    if (_manager.IsUserInRole(user.UserName, rolename))
                    {
                        model.RoleUsers.Add(user.UserName,
                            new RoleUserModel { IsMember = true, UserId = user.UserId, Username = user.UserName });
                    }
                    else
                    {
                        model.RoleUsers.Add(user.UserName,
                            new RoleUserModel { IsMember = false, UserId = user.UserId, Username = user.UserName });
                    }
                }

                return View(model);
            }

            return RedirectToAction("Index", "Role");
        }

        [HttpPost]
        public ActionResult Manage(RoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Manage", model);
            }

            foreach (RoleUserModel roleUser in model.RoleUsers.Values)
            {
                _manager.AddRemoveRoleMember(model.RoleName, roleUser);
            }

            return View("Manage", model);
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
    }
}
