using DatabaseMigrations.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseMigrations.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options) { }

        public DbSet<Audit> Audit { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<Settings> Settings { get; set; }
    }
}
