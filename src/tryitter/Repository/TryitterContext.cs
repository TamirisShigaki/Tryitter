using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository;

public class TryitterContext : DbContext, ITryitterContext
{
    public DbSet<Studant> Studants { get; set; }

    public DbSet<Post> Posts { get; set; }

    public TryitterContext(DbContextOptions<TryitterContext> options) : base(options) { }
    public TryitterContext() { }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(@"
                Server=localhost;
                Database=TryitterDB;
                User Id=sa;
                Password=Password12!;
            ");
        }
    }
}