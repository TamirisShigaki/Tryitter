using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository;

public class TryitterContext : DbContext
{
    public DbSet<Post>? Posts { get; set; }

    public DbSet<Student>? Students { get; set; }

    public TryitterContext(DbContextOptions<TryitterContext> options)
      : base(options) { }

    public TryitterContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = Environment.GetEnvironmentVariable("DOTNET_CONNECTION_STRING");
            
            optionsBuilder.UseSqlServer(@"Server = localhost; Database = TryitterDB; User = sa; Password = 1234; TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
        .HasKey(x => x.StudentId);

        modelBuilder.Entity<Post>()
        .HasKey(x => x.PostId);

        modelBuilder.Entity<Post>()
        .HasOne(c => c.Student)
        .WithMany(x => x.Posts)
        .HasForeignKey(d => d.StudentId);
    }
}