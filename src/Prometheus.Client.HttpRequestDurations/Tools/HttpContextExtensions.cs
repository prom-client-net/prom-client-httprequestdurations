using System;
using Microsoft.AspNetCore.Http;
#if HasRoutes
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
#endif

namespace Prometheus.Client.HttpRequestDurations.Tools
{
    internal static class HttpContextExtensions
    {
#if HasRoutes
        public static string GetRouteName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var endpoint = httpContext.GetEndpoint();

            var result = endpoint?.Metadata.GetMetadata<RouteNameMetadata>()?.RouteName;

            if (string.IsNullOrEmpty(result))
            {
                var routeAttribute = endpoint?.Metadata.GetMetadata<RouteAttribute>();
                var methodAttribute = endpoint?.Metadata.GetMetadata<HttpMethodAttribute>();

                result = $"{routeAttribute?.Template}{methodAttribute?.Template}";
            }

            return result;
        }
#endif
    }
}
