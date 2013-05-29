using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDDEastAnglia
{
    interface IRoleManager
    {
        bool IsUserInRole(string username, string rolename);

        void AddUserToRole(string username, string rolename);

        string[] GetAllRoles();

        bool RoleExists(string rolename);

        void RemoveUserFromRole(string username, string rolename);

        int GetUsersCount(string rolename);

        void DeleteRole(string rolename);

        void CreateRole(string rolename);
    }
}
