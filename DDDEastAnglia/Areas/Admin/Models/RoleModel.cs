using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class RoleModel
    {
        public string RoleName { get; set; }

        public Dictionary<string, bool> Users { get; set; }
    }
}