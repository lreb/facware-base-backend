using Facware.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Facware.Persistence.FluentValidation
{
    public static class DemoFluentMapping
    {
        public static void DemoMapping(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Demo>(entity => {
                entity.HasIndex(x => new {x.Name, x.Stock });
            });
        }

    }
}