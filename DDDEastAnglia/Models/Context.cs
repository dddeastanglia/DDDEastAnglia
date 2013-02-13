using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DDDEastAnglia.Models
{
    public class Context : DbContext
    {
        public Context() :base("DDDEastAnglia")
        {
            
        }

        public DbSet<Session> Sessions { get; set; }
    }
}