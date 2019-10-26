using Newtonsoft.Json;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using Quartz.NET.Web.Database;
using Quartz.NET.Web.Extensions;
using Quartz.NET.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Utility
{
    public class HttpResultful : IJob
    {
        private JobContext JobContext = SqlServerHelper.GetDbContext();
        public Task Execute(IJobExecutionContext context)
        {
            DateTime dateTime = DateTime.Now;
            string parameter = string.Empty;
            string modelType = string.Empty;
            TaskOptions taskOptions = context.GetTaskOptions();
            string httpMessage = "";
            AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;
            if (taskOptions == null)
            {
                FileHelper.WriteFile(FileQuartz.LogPath + trigger.Group, $"{trigger.Name}.txt", "未到找作业或可能被移除", true);
                return Task.CompletedTask;
            }

            if (string.IsNullOrEmpty(taskOptions.ApiUrl) || taskOptions.ApiUrl == "/")
            {
                FileHelper.WriteFile(FileQuartz.LogPath + trigger.Group, $"{trigger.Name}.txt", "未配置url", true);
                return Task.CompletedTask;
            }

            try
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(taskOptions.AuthKey)
                    && !string.IsNullOrEmpty(taskOptions.AuthValue))
                {
                    header.Add(taskOptions.AuthKey.Trim(), taskOptions.AuthValue.Trim());
                }

                if (taskOptions.RequestType?.ToLower() == "get")
                {
                    httpMessage = HttpManager.HttpGetAsync(taskOptions.ApiUrl, header).Result;
                }
                else
                {
                    httpMessage = HttpManager.HttpPostAsync(taskOptions.ApiUrl, null, null, 60, header).Result;
                }
                //获取URL中的ModelId，分析参数
                string requestParams = taskOptions.ApiUrl.Split('?')[1];
                modelType = requestParams.Split('=')[0].Trim();
                parameter = requestParams.Split('=')[1].Trim();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(parameter);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                httpMessage = ex.Message;
            }

            try
            {
                //TODO:将接口返回的数据同步更新到数据库
                HttpResponse response = JsonConvert.DeserializeObject<HttpResponse>(httpMessage);
                ViewTokens viewToken = JobContext.Set<ViewTokens>().Where(item => item.ModelId == parameter)?.FirstOrDefault();
                var isNew = viewToken == null ? true : false;
                var modelTypeEnum = (short)(StringConst.INTEGRATE.Equals(modelType) ? 1 : 0);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"parameter {parameter} isNew:{isNew}");
                Console.ResetColor();
                viewToken = viewToken ?? new ViewTokens
                {
                    Id = Guid.NewGuid().ToString(),
                    ModelId = parameter,
                    UpdateTime = DateTime.Now,
                    Type = modelTypeEnum,
                    ModelName = taskOptions.TaskName,
                    IsActive = true,
                    IsDemo = taskOptions.PreviewType != "preview" ? true : false,
                    JobDesc = taskOptions.JobDescribe,
                    Desc = taskOptions.Describe
                };
                viewToken.UpdateTime = DateTime.Now;
                viewToken.ViewToken = response.Data;
                var res = isNew == true ? JobContext.ViewTokens.Add(viewToken) : JobContext.ViewTokens.Update(viewToken);
                JobContext.SaveChanges();
                string logContent = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}_{dateTime.ToString("yyyy-MM-dd HH:mm:ss")}_{(string.IsNullOrEmpty(httpMessage) ? "OK" : httpMessage)}\r\n";
                FileHelper.WriteFile(FileQuartz.LogPath + taskOptions.GroupName + "\\", $"{taskOptions.TaskName}.txt", logContent, true);
            }
            catch (Exception)
            {
            }
            Console.Out.WriteLineAsync(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + " " + httpMessage);
            return Task.CompletedTask;
        }
    }
}
