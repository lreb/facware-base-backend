using Facware.Models;
using Microsoft.EntityFrameworkCore;

namespace Facware.Data.Access.Seed
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>().HasData(
                new Item { Name = "A"}
                );
        }
    }
}
