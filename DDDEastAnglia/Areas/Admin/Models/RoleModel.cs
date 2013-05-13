using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class RoleModel
    {
        public string RoleName { get; set; }

        public SortedList<string, RoleUserModel> roleUsers { get; set; }
    }

    public class RoleUserModel
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public bool IsMember { get; set; }
    }
}