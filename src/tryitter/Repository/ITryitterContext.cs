using Microsoft.EntityFrameworkCore;
using tryitter.Models;

namespace tryitter.Repository
{
    public interface ITryitterContext
    {
        public DbSet<Studant> Studants { get; set; }

        public DbSet<Post> Posts { get; set; }

        public int SaveChanges();
    }
}