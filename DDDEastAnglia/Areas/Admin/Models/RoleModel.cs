
using System.Collections.Generic;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class RoleModel
    {
        public string RoleName { get; set; }
        public string FeedbackMessage { get; set; }

        public SortedList<string, RoleUserModel> RoleUsers { get; set; }
    }
}