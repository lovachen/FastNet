using AiBao.Entities;
using AspNetCore.Cache;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cts.web.core.Librs;
using cts.web.core;
using AiBao.Mapping;
using AutoMapper;
using Newtonsoft.Json;

namespace AiBao.Services
{
    public class SysUserService : BaseService
    {
        readonly static object addLock = new object();
        readonly static string BY_ID = "ab.services.sys_user.byid-{0}";
        ICacheManager _cacheManager;
        ABDbContext _dbContext;
        IWebHelper _webHelper;
        IMapper _mapper;
        ActivityLogService _activityLogService;

        public SysUserService(ICacheManager cacheManager,
            ABDbContext dbContext,
            IWebHelper webHelper,
            ActivityLogService activityLogService,
            IMapper mapper)
        {
            _activityLogService = activityLogService;
            _mapper = mapper;
            _webHelper = webHelper;
            _dbContext = dbContext;
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 验证登陆时获取
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public (string Salt, string R) GetSalt(string account, int platform = 0)
        {
            using (var trans = _dbContext.Database.BeginTransaction())
            {
                var user = _dbContext.Sys_User.Where(o => o.Account == account && !o.IsDeleted).Select(item => new { Id = item.Id, Salt = item.Salt }).FirstOrDefault();
                if (user == null)
                    return (null, null);
                //删除原有记录再新增
                string r = EncryptorHelper.GetMD5(Guid.NewGuid().ToString());
                _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_UserR] WHERE [UserId]={user.Id} AND [Platform]={platform};");
                _dbContext.Database.ExecuteSqlCommand($"INSERT INTO [Sys_UserR]([Id],[UserId],[R],[Platform])VALUES({CombGuid.NewGuid()},{user.Id},{r},{platform});");
                trans.Commit();
                return (user.Salt, r);
            }
        }

        /// <summary>
        /// 用户登陆验证
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="platform">0：web，1：app</param>
        /// <returns></returns>
        public (bool Status, string Message, Entities.Sys_User User, Entities.Sys_UserJwt Jwt) ValidateUser(string account, string password, int platform = 0)
        {
            var user = _dbContext.Sys_User.Where(o => o.Account == account && !o.IsDeleted).FirstOrDefault();
            if (user == null) return (false, "账号或密码错误", null, null);

            var r_item = _dbContext.Sys_UserR.FirstOrDefault(o => o.UserId == user.Id && o.Platform == platform);
            if (r_item == null)
            {
                return (false, "非法操作，因子不存在，请重试", null, null);
            }

            var pwd = EncryptorHelper.GetMD5((user.Password ?? "") + r_item.R);
            var log = new Sys_UserLogin()
            {
                Id = CombGuid.NewGuid(),
                UserId = user.Id,
                IpAddress = _webHelper.GetIPAddress(),
                LoginTime = DateTime.Now,
                Status = false
            };
            Entities.Sys_UserJwt jwt = null;
            string msg = "账号或密码错误";
            if (password.Equals(pwd, StringComparison.InvariantCultureIgnoreCase))
            {
                log.Status = true;
                msg = "登陆成功";
                user.LastIpAddress = log.IpAddress;
                _dbContext.Sys_UserR.Remove(r_item);
                jwt = new Sys_UserJwt()
                {
                    Jti = EncryptorHelper.GetMD5(Guid.NewGuid().ToString()),
                    Expiration = DateTime.Now.AddDays(30),
                    RefreshToken = EncryptorHelper.GetMD5(Guid.NewGuid().ToString()),
                    Platform = platform,
                    UserId = user.Id
                };
                _dbContext.Sys_UserJwt.Add(jwt);
            }
            _dbContext.Sys_UserLogin.Add(log);
            _dbContext.SaveChanges();
            return (log.Status, msg, user, jwt);
        }

        /// <summary>
        /// 获取已经登陆用户的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Sys_UserMapping GetLogged(Guid userId)
        {
            return _cacheManager.Get<Sys_UserMapping>(String.Format(BY_ID, userId), () =>
            {
                var user = _dbContext.Sys_User.FirstOrDefault(o => o.Id == userId);
                return user != null ? _mapper.Map<Sys_UserMapping>(user) : null;
            });
        }

        /// <summary>
        /// 更改用户资料
        /// </summary>
        /// <param name="user"></param>
        public (bool Status, string Message) UpdateUser(Sys_UserMapping model, Guid modifier)
        {
            var user = _dbContext.Sys_User.Find(model.Id);
            if (user == null) Fail("用户不存在");
            string oldJson = JsonConvert.SerializeObject(user);

            user.MobilePhone = model.MobilePhone;
            user.Email = model.Email;
            user.Name = model.Name;

            string newJson = JsonConvert.SerializeObject(user);

            _dbContext.SaveChanges();
            //记录日志
            _activityLogService.InsertedEntity<Entities.Sys_User>(model.Id, oldJson, newJson, modifier);
            RemoveCache(user.Id);
            return Success("修改成功");
        }

        /// <summary>
        /// 删除用户，删除后最好调用强制下线操作
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="modifier"></param>
        public void Delete(Guid userId, Guid modifier)
        {

            var user = _dbContext.Sys_User.Find(userId);
            if (user == null) return;

            string oldJson = JsonConvert.SerializeObject(user);
            user.DeletedTime = DateTime.Now;
            user.IsDeleted = true;
            _dbContext.SaveChanges();

            string newJson = JsonConvert.SerializeObject(user);
            _activityLogService.DeletedEntity<Entities.Sys_User>(userId, oldJson, newJson, modifier);
            RemoveCache(user.Id);
        }

        /// <summary>
        /// 修改密码，重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <param name="modifier"></param>
        /// <param name="reset">重置密码，只有管理员的操作</param>
        /// <returns></returns>
        public (bool Status, string Message) UpdatePwd(Guid userId, string oldPwd, string newPwd, Guid modifier, bool reset = false)
        {
            var user = _dbContext.Sys_User.Find(userId);
            if (user == null) return (false, "用户不存在");
            string oldJson = JsonConvert.SerializeObject(user);

            if (reset)
            {
                user.Password = EncryptorHelper.GetMD5(user.Account + user.Salt);
            }
            else
            {
                if (user.Password.Equals(oldPwd, StringComparison.InvariantCultureIgnoreCase))
                {
                    user.Password = newPwd;
                }
                else
                {
                    return (false, "原密码错误");
                }
            }
            _dbContext.SaveChanges();
            string newJson = JsonConvert.SerializeObject(user);
            _activityLogService.InsertedEntity<Entities.Sys_User>(userId, oldJson, newJson, modifier);
            return (true, "修改成功");
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public (bool Status, string Message) AddUser(Entities.Sys_User user)
        {
            lock (addLock)
            {
                if (_dbContext.Sys_User.Any(o => o.Account == user.Account && !o.IsDeleted))
                {
                    return Fail("用户账号已经存在");
                }
                _dbContext.Sys_User.Add(user);
                _dbContext.SaveChanges();
                string newJson = JsonConvert.SerializeObject(user);
                _activityLogService.InsertedEntity<Entities.Sys_User>(user.Id, null, newJson, user.Creator);

                return Success("添加成功");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public IPagedList<Sys_UserMapping> AdminSearch(SysUserSearchArg arg, DataTablesParameters parmas)
        {
            var query = _dbContext.Sys_User.Where(o => !o.IsDeleted);

            if (arg != null)
            {
                if (!String.IsNullOrEmpty(arg.q))
                {
                    arg.q = arg.q.Trim();
                    query = query.Where(o => o.Account.Contains(arg.q) || o.Name.Contains(arg.q) || o.MobilePhone.Contains(arg.q) || o.Email.Contains(arg.q));
                }
            }

            #region 排序

            if (!String.IsNullOrEmpty(parmas.OrderName))
            {
                switch (parmas.OrderName)
                {
                    case "Account":
                        if (parmas.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Account);
                        else
                            query = query.OrderBy(o => o.Account);
                        break;
                    case "Name":
                        if (parmas.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.Name);
                        else
                            query = query.OrderBy(o => o.Name);
                        break;
                    case "CreationTime":
                        if (parmas.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.CreationTime);
                        else
                            query = query.OrderBy(o => o.CreationTime);
                        break;
                    case "LastActivityTime":
                        if (parmas.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.LastActivityTime);
                        else
                            query = query.OrderBy(o => o.LastActivityTime);
                        break;
                    case "MobilePhone":
                        if (parmas.OrderDir.Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                            query = query.OrderByDescending(o => o.MobilePhone);
                        else
                            query = query.OrderBy(o => o.MobilePhone);
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

            return PagedList<Sys_UserMapping>.Create<Entities.Sys_User>(query, parmas.PageIndex, parmas.Length, _mapper);
        }

        /// <summary>
        /// 获取角色的用户
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<Sys_UserMapping> GetRoleUsers(Guid roleId)
        {
            var query = from u_r in _dbContext.Sys_UserRole
                        join u in _dbContext.Sys_User on u_r.UserId equals u.Id
                        select new Sys_UserMapping()
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Account = u.Account
                        };
            return query.Distinct().ToList();
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_UserMapping GetUserMapping(Guid id)
        {
            var user = _dbContext.Sys_User.AsNoTracking().FirstOrDefault(o => o.Id == id);
            return user != null ? _mapper.Map<Sys_UserMapping>(user) : null;
        }

        #region 私有方法

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="userId"></param>
        private void RemoveCache(Guid userId)
        {
            _cacheManager.Remove(String.Format(BY_ID, userId));
        }

        #endregion
    }
}
