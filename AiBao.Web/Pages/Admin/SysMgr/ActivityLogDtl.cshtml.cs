using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AiBao.Services;
using AiBao.Mapping;
using AiBao.Entities;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AiBao.Web.Pages.Admin.SysMgr
{
    /// <summary>
    /// 操作日志详情
    /// </summary>
    public class ActivityLogDtlModel : AdminPrmPageModel
    {
        ActivityLogService _activityLogService;

        public ActivityLogDtlModel(ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        public Sys_ActivityLogMapping ActivityLog { get; set; }
        public JObject OldObject { get; set; }
        public JObject NewObject { get; set; }

        public void OnGet(Guid id)
        {
            ActivityLog = _activityLogService.GetActivityLogMapping(id);
            NewJsonValu(ActivityLog);
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        private void NewJsonValu(Sys_ActivityLogMapping activityLog)
        {
            try
            {
                if (!String.IsNullOrEmpty(activityLog.OldValue))
                {
                    OldObject = (JObject)JsonConvert.DeserializeObject(activityLog.OldValue);
                }
                if (!String.IsNullOrEmpty(activityLog.NewValue))
                {
                    NewObject = (JObject)JsonConvert.DeserializeObject(activityLog.NewValue);
                }
            }
            catch (Exception)
            {

            }
        }



    }
}