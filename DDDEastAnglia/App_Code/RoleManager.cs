using System.Web.Security;

// RoleManager possibly not the best name for this class
// Acts as a wrapper around the static System.Web.Security.Roles class
namespace DDDEastAnglia
{
    public class RoleManager
    {
        public bool IsUserInRole(string username, string rolename)
        {
            return Roles.IsUserInRole(username, rolename);
        }

        public void AddUserToRole(string username, string rolename)
        {
            Roles.AddUserToRole(username, rolename);
        }

        public string[] GetAllRoles()
        {
            return Roles.GetAllRoles();
        }

        public bool RoleExists(string rolename)
        {
            return Roles.RoleExists(rolename);
        }

        public void RemoveUserFromRole(string username, string rolename)
        {
            Roles.RemoveUserFromRole(username, rolename);
        }

        public int GetUsersCount(string rolename)
        {
            return Roles.GetUsersInRole(rolename).Length;
        }

        public void DeleteRole(string rolename)
        {
            Roles.DeleteRole(rolename, false);
        }

        public void CreateRole(string rolename)
        {
            Roles.CreateRole(rolename);
        }
    }
}