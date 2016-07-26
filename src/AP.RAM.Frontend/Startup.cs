using System;
using Microsoft.AspNet.Builder;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using AP.RMA.Frontend.CA.Models;
using Microsoft.Data.Entity;
using System.Diagnostics;
using Microsoft.AspNet.Diagnostics;

namespace AP.RMA.Frontend.CA
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
            var configBuilder = new ConfigurationBuilder(appEnv.ApplicationBasePath)
                .AddJsonFile("config.json");
            Configuration = configBuilder.Build();
            AppDomain.CurrentDomain.SetData("newUserId", Guid.NewGuid());
        }


        public IConfiguration Configuration { get; set; }


        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            Debug.WriteLine($"Connection string: {Configuration.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value}");
            services.AddEntityFramework().AddSqlServer().AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetSection("Data").GetSection("DefaultConnection").GetSection("ConnectionString").Value));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorPage();
            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            app.UseStaticFiles();
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
