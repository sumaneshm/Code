using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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

        public LegacyRoute(string[] targetUrls)
        {
            urls = targetUrls;
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        public Task RouteAsync(RouteContext context)
        {
            string requestedUrl = context.HttpContext.Request.Path.Value.TrimEnd('/');

            if (urls.Contains(requestedUrl,StringComparer.OrdinalIgnoreCase))
            {
                context.Handler = async ctx =>
                {
                    HttpResponse response = ctx.Response;
                    byte[] bytes = Encoding.ASCII.GetBytes($"UR:{requestedUrl}");
                    await response.Body.WriteAsync(bytes, 0, bytes.Length);
                };
            }

            return Task.CompletedTask;
        }
    }
}
