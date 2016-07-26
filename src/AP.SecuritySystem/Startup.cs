using System;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Dnx.Runtime;
using Microsoft.Extensions.Configuration;
using AP.SecuritySystem.Models;
using Microsoft.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.PlatformAbstractions;

namespace AP.SecuritySystem
{
    /*public class Options
    {
        public string ConnectionString { get; set; }
        public string UID { get; set; }
    }*/

    public class Startup
    {
        public Startup(IApplicationEnvironment appEnv)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json");
            Configuration = configBuilder.Build();
        }


        public IConfiguration Configuration { get; set; }


        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Debug.WriteLine($"Connection string: {Configuration.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value}");
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value));
                //.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value));
            //services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddIdentity<ApplicationUser, IdentityRole>(i => { i.Password.RequireDigit = false;
                i.Password.RequireLowercase = false;
                i.Password.RequireUppercase = false;
                i.Password.RequireNonLetterOrDigit = false;
                i.Password.RequiredLength = 4;
                }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddMvc();
            services.AddAuthentication();
            services.AddAuthorization(
                options => options.AddPolicy("Supervisor", policy => {
                    policy.RequireClaim("AccessLevel","admin");
                })

                );
        }

        public void Configure(IApplicationBuilder app)
        {

            /*app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            });*/
            //app.UseExceptionHandler("/Home/Error");
            app.UseIISPlatformHandler();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });

        }
    }
}
