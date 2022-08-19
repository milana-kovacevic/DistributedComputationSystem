using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Frontend.Models;

namespace Frontend.Data
{
    public class JobContext : DbContext
    {
        public JobContext (DbContextOptions<JobContext> options)
            : base(options)
        {
        }

        public DbSet<Frontend.Models.Job> Job { get; set; } = default!;

        public DbSet<Frontend.Models.JobResult>? JobResult { get; set; }
    }
}
