using System;
using cts.web.core;
using cts.web.core.MediaItem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using cts.web.core.Model;
using Microsoft.AspNetCore.Http;
using cts.web.core.Mail;
using AspNetCore.Cache;
using AutoMapper;
using AiBao.Mapping;
using AiBao.Entities;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using AiBao.Framework.Security;
using Microsoft.AspNetCore.Builder;

namespace AiBao.Framework
{
    public class AbEngine : Engine, IEngine
    {
        private readonly IHostingEnvironment _hosting;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public AbEngine(IConfiguration configuration, IHostingEnvironment hosting) : base(configuration)
        {
            _hosting = hosting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public override void Initialize(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddRazorPagesOptions(opts =>
                {
                    opts.Conventions.AddPageRoute("/admin/login", "admin");
                });

            //services.AddDbContextPool<ABDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")),100);
            services.AddDbContext<ABDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //程序集依赖注入
            services.AddAssembly("AiBao.Services");

            //ApiController 的模型验证错误返回
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var res = context.ModelState.Where(e => e.Value.Errors.Any())
                    .Select(e => new ApiJsonResult()
                    {
                        code = 1005,
                        msg = e.Value.Errors.First().ErrorMessage
                    }).FirstOrDefault();
                    return new OkObjectResult(res);
                };
            });
            services.AddSingleton<IWebHelper, WebHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IMailProvide, MailProvide>();
            services.AddSingleton<IMediaItemStorage, MediaItemStorage>();
            services.AddScoped<SiteWorkContext>();
            services.AddScoped<SysUserAuthentication>();
            services.AddScoped<WorkContext>();
             
            //启用redis或者内存缓存,默认使用内存缓存
            services.AddRedisOrMemoryCache(Configuration);
            //
            //Mapper.Initialize(x => x.AddProfile<MappingProfile>());
            services.AddAutoMapper(opts =>
            {
                //opts.AddProfile<MappingProfile>();
            });

            //启用JWT
            services.AddJwt(_hosting);

            //API版本
            services.AddApiVersioning(opts =>
            {
                opts.AssumeDefaultVersionWhenUnspecified = true;
            });

            //中文编码 https://docs.microsoft.com/zh-cn/aspnet/core/security/cross-site-scripting?view=aspnetcore-2.1#customizing-the-encoders
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                                               UnicodeRanges.CjkUnifiedIdeographs }));

            //Cookie登陆状态保存设置
            services.AddAuthentication(o =>
            {
                o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opts =>
                {
                    opts.Cookie.HttpOnly = true;
                    opts.LoginPath = "/admin";
                });
            //
            services.AddResponseCompression();
            services.AddResponseCaching();
        }
    }
}
