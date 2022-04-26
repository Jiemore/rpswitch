using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using rpswitch.Data;
using Microsoft.AspNetCore.ResponseCompression;
using rpswitch.Hubs;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace rpswitch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            /*             services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                options.LoginPath = new PathString("/Login");//没登录跳到这个路径
                            }); */
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                   .AddCookie(options =>
                   {
                       options.LoginPath = "/Account/Login";
                       //options.LogoutPath = "/Logout";
                   });
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
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
            app.UseHttpsRedirection();//将 HTTP 请求重定向到 HTTPS
            app.UseStaticFiles();//允许提供静态文件，例如 HTML、CSS、图像和 JavaScript
            app.UseRouting();//将路由匹配添加到中间件管道。
            app.UseCookiePolicy();//Cookie安全
            app.UseAuthentication();//授权用户访问安全资源。
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapDefaultControllerRoute();
                // endpoints.MapGet("/", context =>
                // {
                //     return Task.Run(() => context.Response.Redirect("/Account/Login"));
                // });
                endpoints.MapBlazorHub();//.RequireAuthorization();
                endpoints.MapRazorPages();//.RequireAuthorization(); // 为 Razor 页面配置终结点路由
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapFallbackToPage("/_Host");
            });

        }
    }
}
