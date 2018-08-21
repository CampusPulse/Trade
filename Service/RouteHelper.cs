
namespace CampusPulse.Trade.Service
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Routing;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class RouteHelper
    {
        public static void AddRoutesToConfiguration(IRouteBuilder routeBuilder)
        {
            if (routeBuilder == null)
            {
                throw new ArgumentNullException(nameof(routeBuilder));
            }
            routeBuilder.MapRoute(  "Status","service-status", new { controller = "Status" });
        }
    }
}
