using Microsoft.EntityFrameworkCore;
using TechnicalTest2023.Models;

namespace TechnicalTest2023.DbContext
{
    public class UserContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
