using Microsoft.EntityFrameworkCore;
using Quartz.NET.Web.Database.Maps;
using Quartz.NET.Web.Models;
using Quartz.NET.Web.Utility;

namespace Quartz.NET.Web.Database
{
    public class JobContext:DbContext
    {

        public DbSet<TaskOptions> TaskOptions { get; set; }

        public DbSet<ViewTokens> ViewTokens { get; set; }

        public JobContext(DbContextOptions<JobContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new TaskOptionsMap(modelBuilder.Entity<TaskOptions>());
            new ViewTokenMap(modelBuilder.Entity<ViewTokens>());
            modelBuilder.Entity<TaskOptions>().ToTable(StringConst.TABLE_TASK_OPTIONS);
            modelBuilder.Entity<ViewTokens>().ToTable(StringConst.TABLE_VIEW_TOKENS);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
