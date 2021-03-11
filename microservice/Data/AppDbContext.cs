using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using microservice.Models;
using Microsoft.EntityFrameworkCore;

namespace microservice.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
