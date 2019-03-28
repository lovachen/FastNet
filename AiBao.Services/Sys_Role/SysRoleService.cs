using AiBao.Entities;
using AiBao.Mapping;
using AspNetCore.Cache;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using cts.web.core;
using Newtonsoft.Json;
using AiBao.Services;
using Microsoft.Extensions.Logging;

namespace AiBao.Services
{
    public class SysRoleService : BaseService
    {
        readonly static object lockObj = new object();
        const string MODEL_ALL = "ab.sys.roles.all";
        const string PERMISSION_ALL = "ab.sys.role.permission.all";
        const string USER_ROLES_ALL = "ab.sys.role.userroles.all";

        ABDbContext _dbContext;
        ICacheManager _cacheManager;
        IMapper _mapper;
        ActivityLogService _activityLogService;
        ILogger<SysRoleService> _logger;

        public SysRoleService(ICacheManager cacheManager,
            ABDbContext dbContext,
            IMapper mapper,
            ILogger<SysRoleService> logger,
            ActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
            _mapper = mapper;
            _dbContext = dbContext;
            _cacheManager = cacheManager;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有的roles 并缓存
        /// </summary>
        /// <returns></returns>
        public List<Sys_RoleMapping> GetRoles()
        {
            return _cacheManager.Get<List<Sys_RoleMapping>>(MODEL_ALL, 60 * 24, () =>
              {
                  return _dbContext.Sys_Role.Select(item => new Sys_RoleMapping()
                  {
                      Id = item.Id,
                      CreationTime = item.CreationTime,
                      Name = item.Name,
                      Description=item.Description,
                  }).ToList();
              });
        }

        /// <summary>
        /// 获取所有的roles 并缓存
        /// </summary>
        /// <returns></returns>
        public List<Sys_PermissionMapping> GetRolePermissons()
        {
            return _cacheManager.Get<List<Sys_PermissionMapping>>(PERMISSION_ALL, () =>
            {
                return _dbContext.Sys_Permission
                .Select(item => new Sys_PermissionMapping()
                {
                    Id = item.Id,
                    RoleId = item.RoleId,
                    CategoryId = item.CategoryId
                }).ToList();
            });
        }

        /// <summary>
        /// 获取所有的用户角色 并缓存
        /// </summary>
        /// <returns></returns>
        public List<Sys_UserRoleMapping> GetUserRoles()
        {
            return _cacheManager.Get<List<Sys_UserRoleMapping>>(USER_ROLES_ALL, () =>
            {
                return _dbContext.Sys_UserRole.Select(item => new
                Sys_UserRoleMapping()
                {
                    RoleId = item.RoleId,
                    UserId = item.UserId,
                    Id = item.Id
                }).ToList();
            });
        }

        /// <summary>
        /// 获取用户的角色信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_RoleMapping> GetUserRoles(Guid userId)
        {
            var roles = GetRoles();
            var userRoles = GetUserRoles();
            if (roles != null && userRoles != null)
                return userRoles.Where(o => o.UserId == userId).Join(roles, ur => ur.RoleId, r => r.Id, (ur, r) => r).Distinct().ToList();
            return null;
        }

        /// <summary>
        /// 配置用户角色
        /// </summary>
        /// <param name="userRoles"></param>
        public void SetUserRoles(Guid userId, List<Guid> roleIds, Guid modifier)
        {
            try
            {
                using (var trans = _dbContext.Database.BeginTransaction())
                {
                    _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_UserRole] WHERE [UserId]={userId}");
                    if (roleIds != null && roleIds.Any())
                        roleIds.ForEach(roleId =>
                        {
                            _dbContext.Sys_UserRole.Add(new Sys_UserRole()
                            {
                                Id = CombGuid.NewGuid(),
                                RoleId = roleId,
                                UserId = userId
                            });
                        });
                    _dbContext.SaveChanges();
                    trans.Commit();
                    _cacheManager.Remove(USER_ROLES_ALL);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }


        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <param name="modifier"></param>
        public void DeleteUserRole(Guid userId, Guid roleId, Guid modifier)
        {
            var user_role = _dbContext.Sys_UserRole.FirstOrDefault(o => o.RoleId == roleId && o.UserId == userId);
            if (user_role == null) return;

            string oldLog = JsonConvert.SerializeObject(user_role);

            _dbContext.Sys_UserRole.Remove(user_role);
            _dbContext.SaveChanges();

            _activityLogService.DeletedEntity<Entities.Sys_UserRole>(user_role, oldLog, null, modifier);
        }

        /// <summary>
        /// 从数据库获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Sys_RoleMapping GetRoleMapping(Guid id)
        {
            var item = _dbContext.Sys_Role.AsNoTracking().FirstOrDefault(o => o.Id == id);
            return item == null ? null : _mapper.Map<Sys_RoleMapping>(item);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public (bool Status, string Message) AddRole(Entities.Sys_Role role)
        {
            lock (lockObj)
            {
                if (_dbContext.Sys_Role.Any(o => o.Name == role.Name))
                {
                    return Fail("角色名称已经存在");
                }
                _dbContext.Sys_Role.Add(role);
                _dbContext.SaveChanges();

                string newJson = JsonConvert.SerializeObject(role);
                _activityLogService.InsertedEntity<Entities.Sys_Role>(role.Id, null, newJson, role.Creator);
                RemoveCahce();
                return Success("添加成功");
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public (bool Status, string Message) UpdateRole(Sys_RoleMapping role, Guid modifier)
        {
            lock (lockObj)
            {
                var item = _dbContext.Sys_Role.Find(role.Id);
                if (item == null) return Fail("角色不存在");
                string oldLog = JsonConvert.SerializeObject(item);
                item.Name = role.Name;
                item.Description = role.Description;
                _dbContext.SaveChanges();
                string newLog = JsonConvert.SerializeObject(item);
                _activityLogService.UpdatedEntity<Entities.Sys_Role>(item.Id, oldLog, newLog, modifier);
                RemoveCahce();
                return Success("修改成功");
            }
        }

        /// <summary>
        /// 删除角色，将删除角色所有的权限配置
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public (bool Status, string Message) DeleteRole(Guid roleId, Guid modifier)
        {
            lock (lockObj)
            {
                using (var trans = _dbContext.Database.BeginTransaction())
                {
                    var item = _dbContext.Sys_Role.Find(roleId);
                    if (item == null) return Fail("角色不存在");
                    string oldLog = JsonConvert.SerializeObject(item);

                    _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_Permission] WHERE [RoleId]={item.Id}");
                    _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_UserRole] WHERE [RoleId]={item.Id}");

                    _dbContext.Sys_Role.Remove(item);
                    _dbContext.SaveChanges();
                    _activityLogService.UpdatedEntity<Entities.Sys_Role>(item.Id, oldLog, null, modifier);
                    trans.Commit();

                    //
                    RemoveCahce();
                    Remove_PRM_USER();
                    return Success("删除成功");
                }
            }
        }

        /// <summary>
        /// 获取用户的所有权限数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Sys_PermissionMapping> GetUserPermissions(Guid userId)
        {
            var userRoles = GetUserRoles();
            var rolePermissions = GetRolePermissons();
            if (userRoles != null && rolePermissions != null)
            {
                return userRoles.Where(o => o.UserId == userId)
                      .Join(rolePermissions, ur => ur.RoleId, rp => rp.RoleId, (a, b) => b).Distinct().ToList();
            }
            return null;
        }


        /// <summary>
        /// 配置，角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="categoryIds"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public (bool Status, string Message) SetRolePermission(Guid roleId, List<Guid> categoryIds, Guid modifier)
        {
            lock (lockObj)
            {
                if (!_dbContext.Sys_Role.Any(o => o.Id == roleId))
                {
                    return Fail("角色不存在或已被删除");
                }
                using (var trans = _dbContext.Database.BeginTransaction())
                {
                    _dbContext.Database.ExecuteSqlCommand($"DELETE FROM [Sys_Permission] WHERE [RoleId]={roleId}");
                    categoryIds.ForEach(id =>
                    {
                        _dbContext.Sys_Permission.Add(new Sys_Permission()
                        {
                            Id = CombGuid.NewGuid(),
                            CategoryId = id,
                            RoleId = roleId
                        });
                    });
                    _dbContext.SaveChanges();
                    trans.Commit();
                }
            }
            _cacheManager.Remove(PERMISSION_ALL);
            return Success("保存成功");
        }

        #region 私有方法


        private void RemoveCahce()
        {
            _cacheManager.Remove(MODEL_ALL);
        }

        private void Remove_PRM_USER()
        {
            _cacheManager.Remove(PERMISSION_ALL);
            _cacheManager.Remove(USER_ROLES_ALL);
        }

        #endregion

    }
}
