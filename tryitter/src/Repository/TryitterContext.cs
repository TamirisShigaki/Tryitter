using Microsoft.EntityFrameworkCore;
// using video_portal.Models;

// namespace video_portal.Repository;

public class TryitterContext : DbContext, IVideoPortalContext
{
    // public DbSet<Channel> Channels { get; set; }
    // public DbSet<Comment> Comments { get; set; }
    // public DbSet<User> Users { get; set; }
    // public DbSet<Video> Videos { get; set; }

    public VideoPortalContext(DbContextOptions<VideoPortalContext> options) : base(options) { }
    public TryitterContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"
                Server=localhost,1433;
                Database=TryitterDB;
                User Id=sa;
                Password=Password12!;
            ");
        }
    }
}