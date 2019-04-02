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
    public class BucketImageService
    {
        private readonly static Object lockObj = new object();
        private ABDbContext _dbContext;
        IMapper _mapper;

        public BucketImageService(ABDbContext dbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visitUrl"></param>
        /// <returns></returns>
        public BucketImageMapping GetByVisitUrl(string visitUrl)
        {
            return _dbContext.BucketImage.Select(item => new BucketImageMapping()
            {
                VisitUrl = item.VisitUrl,
                IOPath = item.IOPath,
                ExtName=item.ExtName,
            }).FirstOrDefault(o => o.VisitUrl == visitUrl);
        }

        /// <summary>
        /// 通过sha1获取
        /// </summary>
        /// <param name="sha1"></param>
        /// <returns></returns>
        public BucketImageMapping GetSHA1(string sha1)
        {
            return _dbContext.BucketImage.Select(item => new
            BucketImageMapping()
            {
                SHA1 = item.SHA1,
                VisitUrl = item.VisitUrl,
                IOPath = item.IOPath,
            }).FirstOrDefault(o => o.SHA1 == sha1);
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="bucketImage"></param>
        public void AddImage(Entities.BucketImage bucketImage)
        {
            lock (lockObj)
            {
                if (!_dbContext.BucketImage.Any(o => o.SHA1 == bucketImage.SHA1))
                {
                    _dbContext.BucketImage.Add(bucketImage);
                    _dbContext.SaveChanges();
                }
            }
        }


        public IPagedList<BucketImageMapping> AdminSearch(BucketImageSearchArg arg,DataTablesParameters parameters)
        {
            var query = _dbContext.BucketImage.AsQueryable();

            #region 排序

            if (!String.IsNullOrEmpty(parameters.OrderName))
            {
                switch (parameters.OrderName)
                {
                    case "FileName":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.CreationTime);
                        else
                            query = query.OrderBy(o => o.FileName);
                        break;
                    case "CreationTime":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.CreationTime);
                        else
                            query = query.OrderBy(o => o.CreationTime);
                        break;
                    case "Height":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Height);
                        else
                            query = query.OrderBy(o => o.Height);
                        break;
                    case "Width":
                        if (parameters.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Width);
                        else
                            query = query.OrderBy(o => o.Width);
                        break;
                    default:
                        query = query.OrderBy(o => o.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(o => o.Id);
            }
            #endregion

            return PagedList<BucketImageMapping>.Create<Entities.BucketImage>(query, parameters.PageIndex, parameters.Length, _mapper);

        }







    }
}
