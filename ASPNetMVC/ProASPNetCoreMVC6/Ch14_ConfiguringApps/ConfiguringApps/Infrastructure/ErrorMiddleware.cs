using Microsoft.AspNetCore.Http;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace ConfiguringApps.Infrastructure
{
    public class BrowserTypeMiddleware
    {
        private readonly RequestDelegate nextDelegate;

        public BrowserTypeMiddleware(RequestDelegate next) => nextDelegate = next;

        public async Task Invoke(HttpContext context)
        {
            context.Items["EdgeBrowser"] = context.Request.Headers["User-Agent"]
                .Any(v => v.ToLower().Contains("edge"));

            await nextDelegate.Invoke(context);
        }
    }

    public class ErrorMiddleware
    {
        private readonly RequestDelegate nextDelegate;

        public ErrorMiddleware(RequestDelegate next) => nextDelegate = next;

        public async Task Invoke(HttpContext context)
        {
            await nextDelegate.Invoke(context);

            if(context.Response.StatusCode == 403)
            {
                await context.Response.WriteAsync("Edge is not supported", Encoding.UTF8);
            }
            else if (context.Response.StatusCode == 404)
            {
                await context.Response.WriteAsync("No content middleware found", Encoding.UTF8);
            }

        }
    }


    public class ShortCircuitMiddleware
    {

        private readonly RequestDelegate nextDelegate;

        public ShortCircuitMiddleware(RequestDelegate next) => nextDelegate = next;

        public async Task Invoke(HttpContext context)
        {
            if (context.Items["EdgeBrowser"] as bool? == true)
            {
                context.Response.StatusCode = 403;
            }
            else
                await nextDelegate.Invoke(context);
        }
    }


}
