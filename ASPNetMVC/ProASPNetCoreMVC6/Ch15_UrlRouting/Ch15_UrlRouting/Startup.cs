using Ch15_UrlRouting.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ch15_UrlRouting
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("weekday", typeof(WeekdayRouteConstraint));
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvc(routes => {
                routes.MapRoute(name: "areas", template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.Routes.Add(new LegacyRoute( app.ApplicationServices, new[] { "/aadhavan", "/aghilan" }));

                //routes.MapRoute(name: "NewRoute", template: "App/Do{action}", defaults: new { controller = "Home" });
                //routes.MapRoute(name: "ShopSchema", template: "Shop/{action=Index}", defaults: new { controller = "Home" });
                //routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id:weekday?}");

                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "out", template: "outbounds/{controller=Home}/{action=Index}");
                }
            );
        }
    }
}
