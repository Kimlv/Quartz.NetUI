using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Models
{
    public class HttpResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public bool Success { get; set; }
    }
}
