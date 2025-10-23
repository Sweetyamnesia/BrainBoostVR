using Microsoft.EntityFrameworkCore;
using BrainBoostVR_API.Models;

namespace BrainBoostVR_API.Data
{
    public class BrainBoostDbContext : DbContext
    {
        public BrainBoostDbContext(DbContextOptions<BrainBoostDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }
}
