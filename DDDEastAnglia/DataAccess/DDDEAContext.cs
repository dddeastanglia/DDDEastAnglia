using System.Data.Entity;
using DDDEastAnglia.Models;

namespace DDDEastAnglia.DataAccess
{
    public class DDDEAContext : DbContext
    {
        public DDDEAContext()
            : base("DDDEastAnglia")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}