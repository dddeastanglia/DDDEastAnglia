using System.Data.Entity;
using DDDEastAnglia.DataAccess.EntityFramework.Models;
using DDDEastAnglia.DataModel;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess.EntityFramework
{
    public class DDDEAContext : DbContext
    {
        public DDDEAContext()
            : base("DDDEastAnglia")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Conference> Conferences { get; set; }
    }
}