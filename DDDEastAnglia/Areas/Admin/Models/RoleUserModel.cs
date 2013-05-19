using System;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Areas.Admin.Models
{
    public class RoleUserModel
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        public bool IsMember { get; set; }
    }
}