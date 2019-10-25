using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Quartz.NET.Web.Database;
using Quartz.NET.Web.Models;

namespace Quartz.NET.Web.Controllers
{
    public class ViewTokenController : Controller
    {
        private JobContext jobContext;

        //构造注入数据库上下文
        public ViewTokenController(JobContext context)
        {
            jobContext = context;
        }

        //获取模型当前的ViewToken
        public IActionResult GetModelViewToken(string modelId)
        {
            ViewTokens viewTokens = jobContext.Set<ViewTokens>().Where(item => item.ModelId == modelId)?.FirstOrDefault();
            return Json(new { data = viewTokens.ViewToken, message = "OK", code = 20000, success = true });
        }
    }
}