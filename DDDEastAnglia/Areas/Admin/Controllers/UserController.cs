﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDEastAnglia.DataAccess;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public partial class UserController : Controller
    {
        private readonly DDDEAContext db = new DDDEAContext();
        
        // GET: /Admin/User/
        public virtual ActionResult Index()
        {
            List<UserProfile> profiles = db.UserProfiles.ToList();
            return null;
        }
    }
}
