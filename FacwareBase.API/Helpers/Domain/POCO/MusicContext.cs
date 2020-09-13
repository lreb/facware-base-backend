using Microsoft.EntityFrameworkCore;

namespace FacwareBase.API.Helpers.Domain.POCO
{
    
public class MusicContext: DbContext
{
    public DbSet<Album> Albums{get; set;}
    public DbSet<Song> Songs { get; set; }

    public MusicContext(): base() { }
    public MusicContext(DbContextOptions opts): base(opts) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Album>().HasMany(a => a.Songs).WithOne(s => s.Album); // One-To-Many
        builder.Entity<Album>().HasData(new Album{
            Id = 1, Name = "The Court of the Crimson King"
        });
        // https://wildermuth.com/2018/08/12/Seeding-Related-Entities-in-EF-Core-2-1-s-HasData()
        builder.Entity<Song>().HasData(
            new {Id = 1, AlbumId = 1, Name = "21st Century Schzoid Man"},
            new {Id = 2, AlbumId = 1, Name = "I Talk to the Wind"}
        );
    }
}
}