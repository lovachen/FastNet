using AutoMapper;
using System;

namespace AiBao.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Bucket, BucketMapping>(); 
            CreateMap<BucketMapping, Entities.Bucket>();

            CreateMap<Entities.BucketImage, BucketImageMapping>();
            CreateMap<BucketImageMapping, Entities.BucketImage>();

            CreateMap<Entities.Sys_ActivityLog, Sys_ActivityLogMapping>();
            CreateMap<Sys_ActivityLogMapping, Entities.Sys_ActivityLog>();

            CreateMap<Entities.Sys_ActivityLogComment, Sys_ActivityLogCommentMapping>();
            CreateMap<Sys_ActivityLogCommentMapping, Entities.Sys_ActivityLogComment>();

            CreateMap<Entities.Sys_Category, Sys_CategoryMapping>();
            CreateMap<Sys_CategoryMapping, Entities.Sys_Category>();

            CreateMap<Entities.Sys_NLog, Sys_NLogMapping>();
            CreateMap<Sys_NLogMapping, Entities.Sys_NLog>();

            CreateMap<Entities.Sys_Permission, Sys_PermissionMapping>();
            CreateMap<Sys_PermissionMapping, Entities.Sys_Permission>();

            CreateMap<Entities.Sys_Role, Sys_RoleMapping>();
            CreateMap<Sys_RoleMapping, Entities.Sys_Role>();

            CreateMap<Entities.Sys_User, Sys_UserMapping>();
            CreateMap<Sys_UserMapping, Entities.Sys_User>();

            CreateMap<Entities.Sys_Setting, Sys_SettingMapping>();
            CreateMap<Sys_SettingMapping, Entities.Sys_Setting>();

            CreateMap<Entities.Sys_User, Sys_UserMapping>();
            CreateMap<Sys_UserMapping, Entities.Sys_User>();

            CreateMap<Entities.Sys_UserJwt, Sys_UserJwtMapping>();
            CreateMap<Sys_UserJwtMapping, Entities.Sys_UserJwt>();

            CreateMap<Entities.Sys_UserLogin, Sys_UserLoginMapping>();
            CreateMap<Sys_UserLoginMapping, Entities.Sys_UserLogin>();

            CreateMap<Entities.Sys_UserRole, Sys_UserRoleMapping>();
            CreateMap<Sys_UserRoleMapping, Entities.Sys_UserRole>();

        }
    }

}
