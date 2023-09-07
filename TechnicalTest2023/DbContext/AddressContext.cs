using Microsoft.EntityFrameworkCore;
using TechnicalTest2023.Models;

namespace TechnicalTest2023.DbContext
{
    public class AddressContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public AddressContext(DbContextOptions<AddressContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; } = null!;
    }
}
