using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quartz.NET.Web.Models
{
    public class ViewTokens
    {
        public string Id { get; set; }
        public string ModelId { get; set; }
        public string ModelName { get; set; }
        public string ViewToken { get; set; }
        [Column("JobDescription")]
        public string JobDesc { get; set; }
        [Column("Description")]
        public string Desc { get; set; }
        public short Type { get; set; }
        public bool IsDemo { get; set; }
        public DateTime UpdateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
