using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReversiMvcApp.Models;
using System;

namespace ReversiMvcApp.Data
{
    public class ReversiDbContext : IdentityDbContext
    {
        public ReversiDbContext(DbContextOptions<ReversiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        //public DbSet<Game> Games { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //Convert the AandeBeurt enumeration
        //    modelBuilder
        //        .Entity<Game>()
        //        .Property(s => s.TurnColor)
        //        .HasConversion(
        //            s => s.ToString(),
        //            s => (Color)Enum.Parse(typeof(Color), s));

        //    //Convert the 2D Bord array
        //    modelBuilder
        //        .Entity<Game>()
        //        .Property(s => s.Board)
        //        .HasConversion(
        //            board => JsonConvert.SerializeObject(board),
        //            board => JsonConvert.DeserializeObject<Color[,]>(board));
        //    base.OnModelCreating(modelBuilder);

        //}
    }
}
