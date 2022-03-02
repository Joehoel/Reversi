using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Game> Games { get; set; }
    };
}
