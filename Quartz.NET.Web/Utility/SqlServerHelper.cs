using Microsoft.EntityFrameworkCore;
using Quartz.NET.Web.Database;

namespace Quartz.NET.Web.Utility
{
    public class SqlServerHelper
    {
        public static JobContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<JobContext>();
            optionsBuilder.UseSqlServer("Data Source=GLODON-XUHONGBO;Initial Catalog=QuartzJob;Persist Security Info=True;User ID=sa;Password=zaq1!qaz;Connect Timeout=500;");
            var context = new JobContext(optionsBuilder.Options);
            return context;
        }
    }
}
