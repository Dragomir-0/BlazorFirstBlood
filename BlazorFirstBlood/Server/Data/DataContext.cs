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


        //Create Tables
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
