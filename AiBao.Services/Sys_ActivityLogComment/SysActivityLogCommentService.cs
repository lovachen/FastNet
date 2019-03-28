using AiBao.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using AiBao.Mapping;

namespace AiBao.Services
{
    public class SysActivityLogCommentService
    {
        ABDbContext _dbContext;

        public SysActivityLogCommentService(ABDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public List<Sys_ActivityLogCommentMapping> GetActivityLogComments()
        {
            return _dbContext.Sys_ActivityLogComment.Select(item => new Sys_ActivityLogCommentMapping()
            {
                EntityName = item.EntityName,
                Comment = item.Comment
            }).ToList();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            using (var connection = _dbContext.Database.GetDbConnection())
            {
                connection.Open();
                var table = connection.GetSchema("Tables");
                connection.Close();
                List<string> tables = new List<string>();
                foreach (DataRow row in table.Rows)
                {
                    tables.Add(row["TABLE_NAME"].ToString());
                }
                var ac_comments = _dbContext.Sys_ActivityLogComment.ToList();
                ac_comments.ForEach(del =>
                {
                    if (!tables.Any(o => o == del.EntityName))
                    {
                        _dbContext.Sys_ActivityLogComment.Remove(del);
                    }
                });
                tables.ForEach(name =>
                {
                    if (!ac_comments.Any(o => o.EntityName == name))
                    {
                        _dbContext.Sys_ActivityLogComment.Add(new Sys_ActivityLogComment()
                        {
                            EntityName = name
                        });
                    }
                });
                _dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Update(string name,string value)
        {
            _dbContext.Database.ExecuteSqlCommand($"UPDATE [Sys_ActivityLogComment] SET [Comment]={value} WHERE [EntityName]={name}");
        }

    }
}
