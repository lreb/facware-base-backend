using System;
using Facware.Base.Models.Definitions;
using Microsoft.EntityFrameworkCore;

namespace Facware.Base.Database
{
    public class FacwareBaseContext : DbContext
    {
        public FacwareBaseContext() { }
        /*
        public FacwareBaseContext(DbContextOptions<FacwareBaseContext> options)
            : base(options)
        {
            _ = Database.EnsureCreated();
        }*/


        public DbSet<Item> Item { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>().HasData(  
          new Item  
          {
            ItemId = 1,  
            Name = "Electronics",  
            Price = 1,
            Enabled = true,
            CreatedDate = DateTimeOffset.Now
          },
          new Item  
          {
              ItemId = 2,
              Name = "Electronics",
              Price = 2,
              Enabled = true,
              CreatedDate = DateTimeOffset.Now
          }  
      );;  
    }  

    }
}