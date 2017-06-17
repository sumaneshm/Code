using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ch15_UrlRouting.Infrastructure
{
    public class WeekdayRouteConstraint : IRouteConstraint
    {
        private string[] DaysOfWeek = new[] { "sun", "mon", "tue", "wed", "thu", "fri", "sat" };

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return DaysOfWeek.Contains(values[routeKey].ToString().ToLowerInvariant());
        }
    }
}
