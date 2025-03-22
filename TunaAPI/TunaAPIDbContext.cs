using Microsoft.EntityFrameworkCore;
using TunaAPI.Models;


public class TunaAPIDbContext : DbContext
{

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Song> Songs { get; set; }
    public DbSet<SongGenre> SongGenres { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public TunaAPIDbContext(DbContextOptions<TunaAPIDbContext> context) : base(context)
    {

    }
}
