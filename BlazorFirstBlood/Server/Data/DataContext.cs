using BlazorFirstBlood.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorFirstBlood.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Battle>()
                .HasOne(b => b.Attacker)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Battle>()
                .HasOne(b => b.Opponent)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Battle>()
                .HasOne(b => b.Winner)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserUnit>()
                .Ignore(u => u.ImageUrl);
        }
        //Create Tables
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserUnit> UserUnits { get; set; }
        public DbSet<Battle> Battles { get; set; }
    }
}
