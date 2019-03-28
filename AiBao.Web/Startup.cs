using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AiBao.Framework;
using AiBao.Services;
using cts.web.core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AiBao.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration,
            IHostingEnvironment hosting)
        {
            Configuration = configuration;
            Hosting = hosting;
        }
        /// <summary>
        /// 启用https
        /// </summary>
        private bool UseHttps { get; set; }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Hosting { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            UseHttps = Configuration.GetValue<bool>("UseHttps");
            if (UseHttps)
                services.AddHttpsRedirection(opt => opt.HttpsPort = 443);

            var engine = EngineContext.Create(new AbEngine(Configuration, Hosting));
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services);

            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            InstallService installService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();

            //NLog
            env.ConfigureNLog("nlog.config");
            LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("DefaultConnection");

            //初始化系统菜单
            installService.Install();


        }
    }
}
