using System;
using API.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Convert the AandeBeurt enumeration
            modelBuilder
                .Entity<Game>()
                .Property(s => s.TurnColor)
                .HasConversion(
                    s => s.ToString(),
                    s => (Color)Enum.Parse(typeof(Color), s));

            //Convert the 2D Bord array
            modelBuilder
                .Entity<Game>()
                .Property(s => s.Board)
                .HasConversion(
                    board => JsonConvert.SerializeObject(board),
                    board => JsonConvert.DeserializeObject<Color[,]>(board));
        }
    };
}
