using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch15_UrlRouting.Infrastructure
{
    public class LegacyRoute : IRouter
    {
        private string[] urls;
        private readonly IRouter mvcRoute;

        public LegacyRoute(IServiceProvider services, string[] targetUrls)
        {
            urls = targetUrls;
            mvcRoute = services.GetRequiredService<MvcRouteHandler>();
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public async Task RouteAsync(RouteContext context)
        {
            string requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');

            if (urls.Contains(requestedUrl,StringComparer.OrdinalIgnoreCase))
            {
                context.RouteData.Values["controller"] = "Legacy";
                context.RouteData.Values["action"] = "GetLegacyRoute";
                context.RouteData.Values["legacyUrl"] = requestedUrl;

                await mvcRoute.RouteAsync(context);
            }
        }
    }
}
