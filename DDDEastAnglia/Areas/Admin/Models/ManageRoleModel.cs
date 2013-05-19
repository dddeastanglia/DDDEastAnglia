using System.Collections.Generic;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class ManageRoleModel
    {
        public string RoleName { get; set; }

        public SortedList<string, RoleUserModel> roleUsers { get; set; }
    }
}