using System;
using System.Collections.Generic;
using System.Data.Entity;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.Models;
using System.Linq;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public interface IDDDEAContext
    {
        IList<UserProfile> UserProfiles{get;}
        IList<Session> Sessions{get;}
    }

    public class DDDEAContextWrapper : IDDDEAContext
    {
        private readonly DDDEAContext context;

        public DDDEAContextWrapper(DDDEAContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            
            this.context = context;
        }

        public IList<UserProfile> UserProfiles{get {return context.UserProfiles.ToList();}}
        public IList<Session> Sessions{get {return context.Sessions.ToList();}}
    }

    public class DDDEAContext : DbContext
    {
        public DDDEAContext() : base("DDDEastAnglia")
        {}

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Conference> Conferences { get; set; }
    }
}