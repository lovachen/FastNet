using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Services
{
    public class SysUserLoginService
    {
        ABDbContext _dbContext;
        IMapper _mapper;
        ActivityLogService _activityLogService;

        public SysUserLoginService(ABDbContext dbContext,
            IMapper mapper,
            ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取最新的20条登陆记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_UserLoginMapping> GetLastUserLogins(Guid userId)
        {
            return _dbContext.Sys_UserLogin.Where(o => o.UserId == userId).OrderByDescending(o=>o.LoginTime).Take(20).ToList()
                .Select(item => _mapper.Map<Sys_UserLoginMapping>(item)).ToList();
        }




    }
}
