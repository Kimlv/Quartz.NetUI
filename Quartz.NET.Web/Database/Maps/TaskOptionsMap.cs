using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quartz.NET.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Database.Maps
{
    public class TaskOptionsMap
    {
        public TaskOptionsMap(EntityTypeBuilder<TaskOptions> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);
        }
    }
}
