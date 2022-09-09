using ControlNode.Frontend.Models;
using Microsoft.EntityFrameworkCore;

namespace ControlNode.Frontend.Data
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
            // Setup primary keys & defaults.
            modelBuilder.Entity<Job>().HasKey(e => e.JobId);
            modelBuilder.Entity<Job>().Property(e => e.JobId).ValueGeneratedOnAdd();

            modelBuilder.Entity<JobResult>().HasKey(aj => aj.JobId);

            modelBuilder.Entity<AtomicJob>().HasKey(aj => new { aj.AtomicJobId, aj.JobId });
            modelBuilder.Entity<AtomicJob>().Property(e => e.AtomicJobId).ValueGeneratedOnAdd();

            modelBuilder.Entity<AtomicJobResult>().HasKey(aj => new { aj.AtomicJobId, aj.JobId });

            // Setup relationships.
            modelBuilder.Entity<Job>()
                .HasOne(j => j.JobResult)
                .WithOne(jr => jr.Job)
                .HasForeignKey<JobResult>(jr => jr.JobId);

            modelBuilder.Entity<Job>()
                .HasMany(j => j.AtomicJobs)
                .WithOne(a => a.Job)
                .HasForeignKey(a => a.JobId);

            modelBuilder.Entity<AtomicJob>()
                .HasOne(a => a.AtomicJobResult)
                .WithOne(a => a.AtomicJob)
                .HasForeignKey<AtomicJobResult>(a => new { a.AtomicJobId, a.JobId });
        }

        public DbSet<Job> Job { get; set; } = default!;

        public DbSet<JobResult>? JobResult { get; set; }

        public DbSet<AtomicJob>? AtomicJob { get; set; }

        public DbSet<AtomicJobResult>? AtomicJobResult { get; set; }

        public AtomicJobResult? GetAtomicJobResult(int jobId, int atomicJobId) => AtomicJobResult
            ?.Where(j => j.JobId == jobId && j.AtomicJobId == atomicJobId)
            .SingleOrDefault();
    }
}
