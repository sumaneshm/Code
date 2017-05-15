using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SportsStore
{
    public class Startup
    {
        IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:SportsStoreProducts:ConnectionString"]));

            services.AddMvc();
            services.AddTransient<IProductRepository, EFProductRepository>();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            app.UseMvc(routes =>
            {
                routes.MapRoute("CategoryPage", "{category}/Page{page:int}", new { controller = "Product", action = "List" });
                routes.MapRoute("PageOnly", "Page{page:int}", new { controller = "Product", action = "List", page = 1 });
                routes.MapRoute("CategoryOnly", "{category}", new { controller = "Product", action = "List", page = 1 });
                routes.MapRoute("Empty", "", new { controller = "Product", action = "List", page = 1 });
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
