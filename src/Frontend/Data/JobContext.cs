using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Frontend.Models;
using System.Xml;

namespace Frontend.Data
{
    public class JobContext : DbContext
    {
        public JobContext (DbContextOptions<JobContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Marking Id column as identity column. This column will be automatically generated on add.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }

        public DbSet<Frontend.Models.Job> Job { get; set; } = default!;

        public DbSet<Frontend.Models.JobResult>? JobResult { get; set; }
    }
}
