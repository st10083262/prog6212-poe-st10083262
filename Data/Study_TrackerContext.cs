using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Study_Tracker.Models;

namespace Study_Tracker.Data
{
    public class Study_TrackerContext : DbContext
    {
        public Study_TrackerContext (DbContextOptions<Study_TrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Study_Tracker.Models.User> User { get; set; } = default!;

        public DbSet<Study_Tracker.Models.Module> Module { get; set; }
        public DbSet<Study_Tracker.Models.StudyDate> StudyDate { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseExceptionProcessor();
        }
    }
}
