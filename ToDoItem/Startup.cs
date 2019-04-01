using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ToDoItem.Core.Abstract;
using ToDoItem.Infrastructure.DataAccess;
using ToDoItem.Infrastructure.DataAccess.Repositories;
using ToDoItem.Infrastructure.Identity;

namespace ToDoItem.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.SlidingExpiration = true;
                options.Cookie.Name = "ToDoCookie";
            });

            services.AddDbContext<ToDoItemDbContext>(options => options.UseInMemoryDatabase("ToDoItem"));
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("ToDoItemIdentity"));
            
            services.AddDefaultIdentity<ApplicationUser>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            services.AddAutoMapper();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts(options => options.MaxAge(30).IncludeSubdomains());
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());

            app.UseCsp(opts => opts
                .BlockAllMixedContent()
                .StyleSources(s => s.Self().CustomSources("https://fonts.googleapis.com/"))
                .StyleSources(s => s.UnsafeInline())
                .FontSources(s => s.Self().CustomSources("https://fonts.gstatic.com/"))
                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ScriptSources(s => s.Self().UnsafeInline())
            );

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
