using AiBao.Entities;
using AiBao.Mapping;
using cts.web.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AutoMapper;

namespace AiBao.Services
{

    public class SysNLogService
    {
        ABDbContext _dbContext;
        IMapper _mapper;

        public SysNLogService(ABDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IPagedList<Sys_NLogMapping> AdminSearch(NLogSearchArg arg, DataTablesParameters parameters)
        {
            var query = _dbContext.Sys_NLog.AsQueryable();

            #region 排序

            if (!String.IsNullOrEmpty(parameters.OrderName))
            {
                switch (parameters.OrderName)
                {
                    case "Level":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Level);
                        else
                            query = query.OrderBy(o => o.Level);
                        break;
                    case "LoggedForamt":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Logged);
                        else
                            query = query.OrderBy(o => o.Logged);
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

            var data = query.Select(item => new Sys_NLogMapping()
            {
                Id = item.Id,
                Logged = item.Logged,
                Level = item.Level,
                Message = item.Message
            });
            return PagedList<Sys_NLogMapping>.Create(data, parameters.PageIndex, parameters.Length);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_NLogMapping GetNlog(Guid id)
        {
            var item = _dbContext.Sys_NLog.FirstOrDefault(o => o.Id == id);
            return item != null ? _mapper.Map<Sys_NLogMapping>(item) : null;
        }

    }
}
