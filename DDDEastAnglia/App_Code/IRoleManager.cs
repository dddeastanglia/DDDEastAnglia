using DDDEastAnglia.Areas.Admin.Models;

namespace DDDEastAnglia
{
    public interface IRoleManager
    {
        bool IsUserInRole(string username, string rolename);

        void AddUserToRole(string username, string rolename);

        string[] GetAllRoles();

        bool RoleExists(string rolename);

        void RemoveUserFromRole(string username, string rolename);

        int GetUsersCount(string rolename);

        void DeleteRole(string rolename);

        void CreateRole(string rolename);

        void AddRemoveRoleMember(string rolename, RoleUserModel model);

        string[] GetUsersForRole(string rolename);

        void RenameRole(string oldname, string newname);
    }
}
