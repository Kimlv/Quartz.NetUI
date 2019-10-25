using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quartz.NET.Web.Models;

namespace Quartz.NET.Web.Database.Maps
{
    public class ViewTokenMap
    {
        public ViewTokenMap(EntityTypeBuilder<ViewTokens> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);
        }
    }
}
