using Facware.Data.Access.Seed;
using Facware.Models;
using Microsoft.EntityFrameworkCore;

namespace Facware.Data.Access
{
    public class FacwareDbContext: DbContext
    {
        public FacwareDbContext(DbContextOptions<FacwareDbContext> dbContextOptions) :
            base(dbContextOptions)
        {

        }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Seed();
        }
    }
}
