using Microsoft.AspNetCore.Mvc;
using Quartz.NET.Web.Attr;
using Quartz.NET.Web.Database;
using Quartz.NET.Web.Extensions;
using Quartz.NET.Web.Models;
using Quartz.NET.Web.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quartz.NET.Web.Controllers
{

    public class TaskBackGroundController : Controller
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly JobContext _context;
        public TaskBackGroundController(ISchedulerFactory schedulerFactory,JobContext context)
        {
            this._schedulerFactory = schedulerFactory;
            this._context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/TaskBackGround/Index.cshtml");
        }

        /// <summary>
        /// 获取所有的作业
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetJobs()
        {
            //TODO:从数据库中获取作业列表
            List<TaskOptions> options =  _context.Set<TaskOptions>().ToList();
            return Json(await _schedulerFactory.GetJobs());
        }

        public IActionResult GetJobList()
        {
            //TODO:从数据库中获取作业列表
            List<TaskOptions> options = _context.Set<TaskOptions>().ToList();
            //return Json(await _schedulerFactory.GetJobs());
            return Json(options);
        }

        /// <summary>
        /// 获取作业运行日志
        /// </summary>
        /// <param name="taskName"></param>
        /// <param name="groupName"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult GetRunLog(string taskName, string groupName, int page = 1)
        {
            //TODO:从数据库中获取运行日志
            return Json(FileQuartz.GetJobRunLog(taskName, groupName, page));
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        [TaskAuthor]
        public async Task<IActionResult> AddAsyn(TaskOptions taskOptions)
        {
            //TODO:作业信息入库
            return Json(await taskOptions.AddJob(_schedulerFactory));
        }

        public IActionResult Add(TaskOptions taskOptions)
        {
            return null;
        }
        [TaskAuthor]
        public async Task<IActionResult> Remove(TaskOptions taskOptions)
        {
            //TODO:从数据库中逻辑删除
            return Json(await _schedulerFactory.Remove(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> UpdateAsyn(TaskOptions taskOptions)
        {
            return Json(await _schedulerFactory.Update(taskOptions));
        }
        [TaskAuthor]
        public IActionResult Update(TaskOptions taskOptions)
        {
            //TODO:从数据库中更新
            TaskOptions options = _context.Set<TaskOptions>().SingleOrDefault(s => s.Id == taskOptions.Id);
            options.GroupName = taskOptions.GroupName;
            options.Interval = taskOptions.Interval;
            options.RequestType = taskOptions.RequestType;
            options.ApiUrl = options.ApiUrl;
            options.AuthKey = taskOptions.AuthKey;
            options.AuthValue = taskOptions.AuthValue;
            options.Describe = taskOptions.Describe;
            options.TaskName = taskOptions.TaskName;
            _context.TaskOptions.Update(options);
            _context.SaveChanges();
            return Json(taskOptions);
        }
        [TaskAuthor]
        public async Task<IActionResult> Pause(TaskOptions taskOptions)
        {
            return Json(await _schedulerFactory.Pause(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Start(TaskOptions taskOptions)
        {
            return Json(await _schedulerFactory.Start(taskOptions));
        }
        [TaskAuthor]
        public async Task<IActionResult> Run(TaskOptions taskOptions)
        {
            return Json(await _schedulerFactory.Run(taskOptions));
        }
        [TaskAuthor]
        public Task<IActionResult> Copy(TaskOptions taskOptions)
        {
            //TODO:复制作业
            return null;
        }
    }
}