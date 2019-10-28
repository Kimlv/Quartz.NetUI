using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quartz.NET.Web.Database;
using Quartz.NET.Web.Models;
using Quartz.NET.Web.Utility;

namespace Quartz.NET.Web.Controllers
{
    public class ViewModelController : Controller
    {
        private JobContext jobContext;

        //构造注入数据库上下文
        public ViewModelController(JobContext context)
        {
            jobContext = context;
        }

        [AllowAnonymous]
        //获取模型当前的ViewToken
        public IActionResult Index(int? type ,int pageSize = 20 , int pageIndex = 1)
        {
            var ip = NetWork.GetInternalIP();
            ViewBag.Ip = ip.ToString();
            int? _type = type == null ? 2 : type;
            List<ViewTokens> viewTokens = null;
            int? pageTotal = jobContext.Set<ViewTokens>()?.Count(item => item.IsActive);
            switch (_type)
            {
                case 0:
                    viewTokens = jobContext.Set<ViewTokens>().Where(item => !item.IsDemo && item.IsActive)?.Skip(pageSize*(pageIndex-1)).Take(pageSize).ToList();
                    break;
                case 1:
                    viewTokens = jobContext.Set<ViewTokens>().Where(item => item.IsDemo && item.IsActive)?.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    break;
                case 2:
                    viewTokens = jobContext.Set<ViewTokens>().Where(item => item.IsActive)?.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    break;
                default:
                    break;
            }

            return View(viewTokens);
        }
    }
}