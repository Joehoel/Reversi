using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReversiMvcApp.Data
{
    public class ReversiDbContext : IdentityDbContext
    {
        public ReversiDbContext(DbContextOptions<ReversiDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
