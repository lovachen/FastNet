using AiBao.Framework.Model;
using AiBao.Services;
using cts.web.core.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AiBao.Framework.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class SysUserAuthentication
    {
        readonly static string USER_DATA_ITEMS_KEY = "ab.items.user.data";

        IHttpContextAccessor _httpContextAccessor;
        SysUserJwtService _sysUserJwtService;
        IJWTFactory _jwtFactory;

        public SysUserAuthentication(IHttpContextAccessor httpContextAccessor,
            SysUserJwtService sysUserJwtService,
            IJWTFactory jwtFactory)
        {
            _jwtFactory = jwtFactory;
            _sysUserJwtService = sysUserJwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 保存登陆状态
        /// 为了和jwt保存同步 同用JwtRegisteredClaimNames.Jti
        /// </summary>
        /// <param name="jti"></param>
        /// <param name="user"></param>
        /// <param name="expires"></param>
        /// <param name="platform">0：web后台，1：app</param>
        /// <returns>当jwt标识登陆时返回string</returns>
        public string SignIn(string jti, Entities.Sys_User user, DateTime expires, int platform = 0)
        {
            string userDataJson = JsonConvert.SerializeObject(new UserData() { Id = user.Id, Name = user.Name, Account = user.Account, IsAdmin = user.IsAdmin });

            switch (platform)
            {
                case 0:
                    List<Claim> claims = new List<Claim>() {
                        new Claim(JwtRegisteredClaimNames.Jti, jti),
                        new Claim(ClaimTypes.Sid,user.Id.ToString()),
                        new Claim(ClaimTypes.UserData, userDataJson) };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
                    {
                        ExpiresUtc = expires
                    });
                    break;
                case 1:
                    return _jwtFactory.CreateToken(new User() { PrimarySid = platform, UserID = user.Id.ToString(), UserData = userDataJson, UserName = user.Name }, jti, expires);
            }
            return null;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void SignOut(int platform)
        {
            if (platform == 0)
                _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var jti = GetJti(platform);
            if (!String.IsNullOrEmpty(jti))
            {
                _sysUserJwtService.SignOut(jti);
            }
        }

        /// <summary>
        /// 验证是否登陆
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public bool VerifyLogged(int platform)
        {
            string jti = GetJti(platform);

            if (!String.IsNullOrEmpty(jti))
            {
                var jwt = _sysUserJwtService.GetJwtMapping(jti);
                return jwt != null && jwt.Expiration > DateTime.Now;
            }
            return false;
        }

        /// <summary>
        /// 获取jti
        /// </summary>
        /// <returns></returns>
        public string GetJti(int platform)
        {
            return GetClaims(platform, JwtRegisteredClaimNames.Jti)?.Value;
        }

        /// <summary>
        /// 获取存储在登陆票据里面的用户信息
        /// </summary>
        /// <returns></returns>
        public UserData GetData(int platform)
        {
            var userData = _httpContextAccessor.HttpContext.Items[USER_DATA_ITEMS_KEY] as UserData;
            if (userData == null)
            {
                Claim claim = GetClaims(platform, ClaimTypes.UserData);
                if (claim != null)
                {
                    userData = JsonConvert.DeserializeObject<UserData>(claim.Value);
                    if (userData != null)
                        _httpContextAccessor.HttpContext.Items[USER_DATA_ITEMS_KEY] = userData;
                }
            }
            return userData;
        }

        /// <summary>
        /// 获取用户id
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public Guid GetUserId(int platform = 0)
        {
            Guid _id = Guid.Empty;
            var claim = GetClaims(platform, ClaimTypes.Sid);
            if (claim != null)
            {
                if (!String.IsNullOrEmpty(claim.Value))
                {
                    Guid.TryParse(claim.Value, out _id);
                }
            }
            return _id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="claimType"></param>
        /// <returns></returns>
        private Claim GetClaims(int platform, string claimType)
        {
            switch (platform)
            {
                case 0:
                    AuthenticateResult res = _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
                    if (res.Succeeded)
                    {
                        var claim = res.Principal.FindFirst(o => o.Type == claimType);
                        return claim;
                    }
                    break;
                case 1:
                    if (_httpContextAccessor.HttpContext.User != null)
                    {
                        var user = _httpContextAccessor.HttpContext.User;
                        if (user.Claims != null && user.Claims.Any())
                        {
                            return user.Claims.FirstOrDefault(o => o.Type == claimType);
                        }
                    }
                    break;
            }
            return null;
        }


    }
}
