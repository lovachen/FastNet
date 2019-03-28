using AiBao.Entities;
using AiBao.Mapping;
using AutoMapper;
using cts.web.core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AiBao.Services
{
    public class ActivityLogService
    {
        private ABDbContext _dbContext;
        IMapper _mapper;

        public ActivityLogService(IMapper mapper,
            ABDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #region 操作

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logService"></param>
        /// <param name="entiy"></param>
        public void InsertedEntity<T>(object primaryKey, string oldValue, string newValue, Guid? userId = null)
        {
            InsertActivityLog<T>("新增", primaryKey, oldValue, newValue, userId);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logService"></param>
        /// <param name="entiy"></param>
        public void UpdatedEntity<T>(object primaryKey, string oldValue, string newValue, Guid? userId = null)
        {
            InsertActivityLog<T>("修改", primaryKey, oldValue, newValue, userId);
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logService"></param>
        /// <param name="entiy"></param>
        public void DeletedEntity<T>(object primaryKey, string oldValue, string newValue, Guid? userId = null)
        {
            InsertActivityLog<T>("删除", primaryKey, oldValue, newValue, userId);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public IPagedList<Sys_ActivityLogMapping> AdminSearch(ActivityLogSearchArg arg, DataTablesParameters parameters)
        {

            var query = from log in _dbContext.Sys_ActivityLog
                        join u in _dbContext.Sys_User on log.Creator equals u.Id
                        join c in _dbContext.Sys_ActivityLogComment on log.EntityName equals c.EntityName into temp
                        from lcomment in temp.DefaultIfEmpty()
                        select new Sys_ActivityLogMapping()
                        {
                            Id = log.Id,
                            EntityName = log.EntityName,
                            Method = log.Method,
                            NewValue = log.NewValue,
                            OldValue = log.OldValue,
                            CreationTime = log.CreationTime,
                            PrimaryKey = log.PrimaryKey,
                            UserName = u.Name,
                            UserAccount = u.Account,
                            Comment = lcomment.Comment
                        };



            #region 排序

            if (!String.IsNullOrEmpty(parameters.OrderName))
            {
                switch (parameters.OrderName)
                {
                    case "Method":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Method);
                        else
                            query = query.OrderBy(o => o.Method);
                        break;
                    case "EntityName":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.EntityName);
                        else
                            query = query.OrderBy(o => o.EntityName);
                        break;
                    case "CreationTimeForamt":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.CreationTime);
                        else
                            query = query.OrderBy(o => o.CreationTime);
                        break;
                    case "UserAccount":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.UserAccount);
                        else
                            query = query.OrderBy(o => o.UserAccount);
                        break;
                    case "UserName":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.UserName);
                        else
                            query = query.OrderBy(o => o.UserName);
                        break;
                    default:
                        query = query.OrderBy(o => o.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(o => o.Id);
            }
            #endregion

            return PagedList<Sys_ActivityLogMapping>.Create(query, parameters.PageIndex, parameters.Length);
        }

        /// <summary>
        /// 获取用户最新的20条操作日志
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_ActivityLogMapping> GetLastUserActivityLogs(Guid userId)
        {
            return _dbContext.Sys_ActivityLog.Where(o => o.Creator == userId).OrderByDescending(o => o.Creator).Take(20)
                .ToList().Select(item => new Sys_ActivityLogMapping()
                {
                    Id = item.Id,
                    CreationTime = item.CreationTime,
                    EntityName = item.EntityName,
                    Method = item.Method,
                    Comment = item.Comment,
                }).ToList();
        }

        /// <summary>
        /// 获取日志的详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_ActivityLogMapping GetActivityLogMapping(Guid id)
        {
            var query = from log in _dbContext.Sys_ActivityLog.Where(o => o.Id == id)
                        join u in _dbContext.Sys_User on log.Creator equals u.Id
                        join c in _dbContext.Sys_ActivityLogComment on log.EntityName equals c.EntityName into temp
                        from lcomment in temp.DefaultIfEmpty()
                        select new Sys_ActivityLogMapping()
                        {
                            Id = log.Id,
                            EntityName = log.EntityName,
                            Method = log.Method,
                            NewValue = log.NewValue,
                            OldValue = log.OldValue,
                            CreationTime = log.CreationTime,
                            PrimaryKey = log.PrimaryKey,
                            UserName = u.Name,
                            UserAccount = u.Account,
                            Comment = lcomment.Comment
                        };
            return query.FirstOrDefault();
        }

        #region 私有方法

        /// <summary>
        /// 私有方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="logService"></param>
        /// <param name="method"></param>
        /// <param name="entity"></param>
        public void InsertActivityLog<T>(string method, object primaryKey, string oldValue, string newValue, Guid? userId = null)
        {
            try
            {
                var log = new Entities.Sys_ActivityLog()
                {
                    Id = CombGuid.NewGuid(),
                    PrimaryKey = primaryKey.ToString(),
                    CreationTime = DateTime.Now,
                    Method = method,
                    OldValue = oldValue,
                    NewValue = newValue,
                    EntityName = typeof(T).Name,
                    Creator = userId
                };
                _dbContext.Sys_ActivityLog.Add(log);
                _dbContext.SaveChanges();
            }
            catch
            {

            }
        }


        #endregion

    }
}
