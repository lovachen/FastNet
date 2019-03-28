using AiBao.Framework.Filters;
using AiBao.Framework.Security;
using System;
using System.Collections.Generic;
using System.Text;
using cts.web.core;

namespace AiBao.Framework.Pages
{
    [SysUserAuth]
    public abstract class AdminPageModel : BasePageModel
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkContext WorkContext;

        /// <summary>
        /// 当前用户ID
        /// </summary>
        public Guid UserId;

        public AdminPageModel()
        {
            WorkContext = EngineContext.Current.Resolve<WorkContext>();
            UserId = WorkContext.GetUserId(0);
        }
    }
}
