﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quartz.NET.Web.Attr;
using Quartz.NET.Web.Utility;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Quartz.NET.Web.Filters
{
    public class TaskAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        public TaskAuthorizeFilter(IHttpContextAccessor accessor, IConfiguration memoryCache)
        {
            this._accessor = accessor;
            this._configuration = memoryCache;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            WriteLog();
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return;
            if (((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo
                .CustomAttributes.Any(x => x.AttributeType == typeof(TaskAuthorAttribute))
                && context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) != _configuration["superToken"])
            {
                context.Result = new ContentResult()
                {
                    Content = JsonConvert.SerializeObject(new
                    {
                        status =false,
                        msg = "普通帐号不能进行此操作！可通过appsettings.json节点superToken获取管理员帐号。"
                    }),
                    ContentType = StringConst.JSON_CONTENT_TYPE,
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }

        private void WriteLog()
        {
            try
            {
                var request = _accessor.HttpContext.Request;
                string url = new StringBuilder()
                     .Append(request.Scheme)
                     .Append("://")
                     .Append(request.Host)
                     .Append(request.PathBase)
                     .Append(request.Path)
                     .Append(request.QueryString)
                     .ToString();
                FileQuartz.WriteAccess(HttpManager.GetUserIP(_accessor) + "_" + url);
            }
            finally { }
        }
    }
}
