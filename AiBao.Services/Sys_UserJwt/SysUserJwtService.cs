using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Services
{
    public class SysUserJwtService
    {
        readonly static string KEY_BY_JTI = "ab.services.jwt-{0}";

        ICacheManager _cacheManager;
        ABDbContext _dbContext;
        IMapper _mapper;

        public SysUserJwtService(ICacheManager cacheManager,
            ABDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jti"></param>
        /// <returns></returns>
        public Sys_UserJwtMapping GetJwtMapping(string jti)
        {
            return _cacheManager.Get<Sys_UserJwtMapping>(String.Format(KEY_BY_JTI, jti), () =>
            {
                var item = _dbContext.Sys_UserJwt.FirstOrDefault(o => o.Jti == jti);
                return item != null ? _mapper.Map<Sys_UserJwtMapping>(item) : null;
            });
        }

        /// <summary>
        /// 退出登陆，移除jwttoken
        /// </summary>
        /// <param name="jti"></param>
        public void SignOut(string jti)
        {
            _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_UserJwt] WHERE [Jti]={jti}");
            RemoveCahce(jti);
        }

        /// <summary>
        /// 强制用户下线所有平台
        /// </summary>
        /// <param name="userId"></param>
        public void CompelOut(Guid userId)
        {
            var jwtList = _dbContext.Sys_UserJwt.Where(o => o.UserId == userId).ToList();
            _dbContext.Sys_UserJwt.RemoveRange(jwtList);
            _dbContext.SaveChanges();
            jwtList.ForEach(item =>
             {
                 RemoveCahce(item.Jti);
             });
        }

        /// <summary>
        /// 获取用户的所有登陆Jwt记录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_UserJwtMapping> GetLastUserJwt(Guid userId)
        {
           return _dbContext.Sys_UserJwt.Where(o => o.UserId == userId).OrderByDescending(o=>o.Expiration)
                .Select(item => _mapper.Map<Sys_UserJwtMapping>(item)).ToList();
        }


        #region 私有方法

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jti"></param>
        private void RemoveCahce(string jti)
        {
            _cacheManager.Remove(String.Format(KEY_BY_JTI, jti));

        }


        #endregion

    }
}
